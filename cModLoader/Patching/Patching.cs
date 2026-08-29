using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Specialized;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cModLoader.Patching
{
    /// <summary> Some utils to help do stuff regarding patching </summary>
    public static class PatchUtils
    {
        public static Instruction _FindIL(this ILProcessor il, OpCode?[] codes, string[] reference, int offset = 0) {

            var inst = il.Body.Instructions;

            int CheckOp(object op, string cmp) {
                if (cmp == null || op == null) return 10; // idk
                if (op.ToString() == cmp) return 11;
                var type = op.GetType();
                if (type == typeof(MethodDefinition) && cmp == (op as MethodDefinition).Name) return 1;
                if (type == typeof(FieldDefinition) && cmp == (op as FieldDefinition).Name) return 2;
                if (type == typeof(MethodReference) && cmp == (op as MethodReference).Name) return 3;
                if (type == typeof(FieldReference) && cmp == (op as FieldReference).Name) return 4;
                return 0;
            }

            int j = 0;
            for (int i = 0; i < inst.Count; i++) {
                var _il = inst[i];
                var code = _il.OpCode;
                var op = _il.Operand;
                if (code == null || code == codes[j]) {
                    int n = 0;
                    if ((n = CheckOp(op, reference[j])) > 0) {
                        //if (j > 0) Accessibility.Show($"i:{i}/{inst.Count} - Pass {j}\n{op.GetType()}\n{op ?? op.ToString()} == {reference[j]}\nCheck \"n\": {n}");
                        j++;
                    } else if (j != 0) {
                        //Accessibility.Show($"i:{i}/{inst.Count} - Failed {j}\n{op.GetType()}\n{op ?? op.ToString()} == {reference[j]}\nCheck \"n\": {n}");
                        j = 0;
                    }
                }
                else if (j != 0) {
                    //Accessibility.Show($"i:{i}/{inst.Count} - Cleared {j}\n{op.GetType()}\n{op ?? op.ToString()} == {reference[j]}\n");
                    j = 0;
                }
                if (j == codes.Length) {
                    //Accessibility.Show($"i:{i}/{inst.Count} - IL: {(i + offset)}");
                    return inst[i + offset];
                }
            }
            //Accessibility.Show("IL: null");
            return null;

        }
        public static void _CreateCall(this ILProcessor il, Mono.Cecil.Cil.MethodBody body, ModuleDefinition asm, MethodBase method, out Instruction call, out List<Instruction> arg) {
            var hookMethod = asm.ImportReference(method);
            arg = new List<Instruction>();
            if (!body.Method.IsStatic)
                arg.Add(il.Create(OpCodes.Ldarg_0)); // "this"
            for (int i = 0; i < body.Method.Parameters.Count; i++)
                arg.Add(il.Create(OpCodes.Ldarg, i + (body.Method.IsStatic ? 0 : 1))); // add opp to push params
            call = il.Create(OpCodes.Call, hookMethod);
        }

    }

    /// <summary>
    /// A custom patcher class used to patch, as Harmony and MonoMod seems to incorrectly create IL code causing crashes, see (https://github.com/pardeike/Harmony/issues/640) for a related topic.<br/>
    /// This is an abstract method so you can create an instance of it which contains some stuff like <see cref="AddPatch"/>
    /// </summary>
    public static class cModPatch {
        /// <summary>
        /// Used to store information about a patch, nothing in this class should be modified by the mod as its fields are used for referencing, changing fields could result in a crash.
        /// </summary>
        public class Patch
        {
            /// <summary> Array of user defined IL modifications. Changing this in any way will do nothing, as its just for referencing by the mod. </summary>
            public ILModification ilModifications { get; internal set; }
            /// <summary> Was the patch successful </summary>
            public bool Patched { get; internal set; }
            /// <summary> The class the patch belongs to </summary>
            public string ClassName { get; internal set; }
            /// <summary> The name of the new function </summary>
            public string FunctionName { get; internal set; }
            /// <summary> The patch return type </summary>
            public string ReturnType { get; internal set; }
            /// <summary> The patch return type </summary>
            public int ParameterCount { get; internal set; }
            /// <summary> The patch return type </summary>
            public string[] ParameterTypes { get; internal set; }
            /// <summary> The new function you created for this patch </summary>
            public MethodInfo wrapperFunction { get; internal set; }
            /// <summary> The name of the function originally being patched </summary>
            public string originalFunctionName { get; internal set; }
            /// <summary> The function that was patched </summary>
            public MethodInfo originalFunction { get; internal set; }
            /// <summary> The code name identifier for the patch's origin function </summary>
            public string PatchOrigin { get; internal set; }
            /// <summary> The code name identifier for the patch </summary>
            public string PatchFullName { get; internal set; }

            // use AddPatch instead, the regex is not complete [Its been removed now] and the only use for this is adding to `Patchs` which is also internal
            internal Patch(string Class, string FunctionName, string ReturnType, string[] ParameterTypes, MethodInfo NewFunction, ILModification ilModifications, bool addPatch) {
                this.ClassName = Class;
                this.FunctionName = FunctionName;
                this.ReturnType = ReturnType;
                this.ParameterCount = ParameterTypes.Length;
                this.ParameterTypes = ParameterTypes;
                this.wrapperFunction = NewFunction;
                this.ilModifications = ilModifications;
                if (addPatch) ModContent.patches.Add(this);
            }
        }
        /// <summary>  </summary>
        // idk why instructions is an IList, a List might work but my past self might have known something i currently don't
        public delegate void ILModification(Mono.Cecil.Cil.MethodBody body, ILProcessor il, IList<Instruction> instructions, ModuleDefinition terrariaModule);

        // ChatGPT helped with this
        internal static MethodDefinition CloneMethod(ModuleDefinition module, MethodDefinition sourceMethod, string newName)
        {
            // clone attributes but make public for convenience
            var attributes = sourceMethod.Attributes;
            attributes &= ~Mono.Cecil.MethodAttributes.MemberAccessMask; // clear visibility bits (bitmask 0x0007)
            attributes |= Mono.Cecil.MethodAttributes.Public; // make public
            // create new method
            var newMethod = new MethodDefinition(newName, attributes, module.ImportReference(sourceMethod.ReturnType));

            // clone params
            foreach (var param in sourceMethod.Parameters)
                newMethod.Parameters.Add(
                    new ParameterDefinition(param.Name, param.Attributes, module.ImportReference(param.ParameterType))
                );

            // clone local vars
            foreach (var variable in sourceMethod.Body.Variables)
                newMethod.Body.Variables.Add(
                    new VariableDefinition(module.ImportReference(variable.VariableType))
                );

            // copy "InitLocals" flag (required for initializing local variables properly)
            newMethod.Body.InitLocals = sourceMethod.Body.InitLocals;

            var il = newMethod.Body.GetILProcessor();
            var instructionMap = new Dictionary<Instruction, Instruction>();

            // clone instructions and keep track of mapping from old to new
            foreach (var instr in sourceMethod.Body.Instructions)
            {
                // get intruction clone
                // for some reason you cant just clone it, you need to do a bunch of other stuff
                Instruction clone = null;
                if (instr.Operand == null) clone = Instruction.Create(instr.OpCode);
                else
                {
                    // convert Definitions to References when needed (ChatGPT said this will suffice, i guess we will see)
                    if (instr.Operand is FieldDefinition fieldDef)          instr.Operand = module.ImportReference(fieldDef);
                    else if (instr.Operand is MethodDefinition methodDef)   instr.Operand = module.ImportReference(methodDef);
                    else if (instr.Operand is TypeDefinition typeDef)       instr.Operand = module.ImportReference(typeDef);

                    // this used to use dynamic but that seems to have issues in other places on Linux maybe so to be proactive we wont use it
                    var op = instr.Operand;
                    if (op is TypeReference type)                   clone = Instruction.Create(instr.OpCode, type);
                    else if (op is CallSite site)                   clone = Instruction.Create(instr.OpCode, site);
                    else if (op is MethodReference method)          clone = Instruction.Create(instr.OpCode, method);
                    else if (op is FieldReference field)            clone = Instruction.Create(instr.OpCode, field);
                    else if (op is string value_string)             clone = Instruction.Create(instr.OpCode, value_string);
                    else if (op is sbyte value_sbyte)               clone = Instruction.Create(instr.OpCode, value_sbyte);
                    else if (op is byte value_byte)                 clone = Instruction.Create(instr.OpCode, value_byte);
                    else if (op is int value_int)                   clone = Instruction.Create(instr.OpCode, value_int);
                    else if (op is long value_long)                 clone = Instruction.Create(instr.OpCode, value_long);
                    else if (op is float value_float)               clone = Instruction.Create(instr.OpCode, value_float);
                    else if (op is double value_double)             clone = Instruction.Create(instr.OpCode, value_double);
                    else if (op is Instruction target)              clone = Instruction.Create(instr.OpCode, target);
                    else if (op is Instruction[] targets)           clone = Instruction.Create(instr.OpCode, targets);
                    else if (op is VariableDefinition variable)     clone = Instruction.Create(instr.OpCode, variable);
                    else if (op is ParameterDefinition parameter)   clone = Instruction.Create(instr.OpCode, parameter);
                    else throw new NotSupportedException($"Unsupported operand type: {instr.Operand.GetType().FullName} for opcode {instr.OpCode}");
                }

                il.Append(clone);
                instructionMap[instr] = clone;
            }

            // fix operand references to other instructions
            foreach (var instr in newMethod.Body.Instructions)
            {
                if (instr.Operand is Instruction target)
                    instr.Operand = instructionMap[target];
                else if (instr.Operand is Instruction[] targets)
                    instr.Operand = targets.Select(t => instructionMap[t]).ToArray();
            }

            // clone exception handlers and remap instruction references
            foreach (var eh in sourceMethod.Body.ExceptionHandlers)
            {
                newMethod.Body.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType)
                {
                    TryStart = instructionMap[eh.TryStart],
                    TryEnd = instructionMap[eh.TryEnd],
                    HandlerStart = instructionMap[eh.HandlerStart],
                    HandlerEnd = instructionMap[eh.HandlerEnd],
                    CatchType = eh.CatchType != null ? module.ImportReference(eh.CatchType) : null,
                    FilterStart = eh.FilterStart != null ? instructionMap[eh.FilterStart] : null
                });
            }

            return newMethod;
        }
        internal static Dictionary<MethodDefinition, int> funcPatchCount = new Dictionary<MethodDefinition, int>();
        internal static byte[] LoadAndPatchTerraria(string realTerrariaPath)
        {
            var resolver = new DefaultAssemblyResolver();
            resolver.ResolveFailure += (o, e) => { // returns AssemblyDefinition
                if (cModLoaderInitializer.LoadedAssembilies.TryGetValue(e.Name, out var asm)) {
                    Output.Print("Mono.Cecil ResolveFailure, loading from loaded assemblies.");
                    return AssemblyDefinition.ReadAssembly(asm.Location);
                }
                var asms = string.Join("\n", cModLoaderInitializer.LoadedAssembilies.ToList().Select(x => $"{x.Key}: {x.Value.FullName}"));
                Accessibility.Show($"Mono.Cecil failed to load assembily \"{e.FullName}\".\ncModLoader will crash. Loaded assemblies:\n{asms}");
                return null;
            };

            ModuleDefinition moduleDef = ModuleDefinition.ReadModule(realTerrariaPath, new ReaderParameters { AssemblyResolver = resolver });

            Output.Print("Do patches > " + ModContent.patches.Count);

            foreach (var patch in ModContent.patches)
            {
                patch.Patched = false;
                patch.PatchOrigin = $"{patch.ReturnType} {patch.ClassName}::{patch.FunctionName}({patch.ParameterCount})";
                var wf = patch.wrapperFunction;

                patch.PatchFullName = wf == null ? $"{patch.PatchOrigin} => {patch.PatchOrigin}" : $"{patch.PatchOrigin} => {wf.ReturnType.FullName} {wf.DeclaringType.FullName}::{wf.Name}({wf.GetParameters().Length})";
                Output.Print($"Patch \"{patch.PatchFullName}\"");

                var classType = moduleDef.GetType(patch.ClassName);
                if (classType == null)
                {
                    Output.Print($" > Could not find class \"{patch.ClassName}\".");
                    continue;
                }
                // classType.Methods.First did not work when no elements exist
                MethodDefinition func = null; // classType.Methods.First(m => m.Name == patch.FunctionName && m.ReturnType.FullName == patch.ReturnType && m.Parameters.Count == patch.ParameterCount);
                int index = -1;
                int count = 0;
                for (int i = 0; i < classType.Methods.Count; i++) {
                    var m = classType.Methods[i];
                    if (m.Name == patch.FunctionName && m.ReturnType.FullName == patch.ReturnType && m.Parameters.Count == patch.ParameterCount) {
                        var pass = true;
                        for (int j = 0; j < patch.ParameterCount; j++) {
                            if (patch.ParameterTypes[j] != m.Parameters[j].ParameterType.FullName) {
                                pass = false;
                                break;
                            }
                        }
                        if (pass) func = m;
                    }
                }

                // IL modification will modify func if wf is null
                if (func == null)
                {
                    Output.Print($" > Could not find function \"{patch.FunctionName}\" in \"{patch.ClassName}\" with {patch.ParameterCount} parameters.");
                    continue;
                }

                // if wf is null it will do IL patches on `func` instead of the clone
                if (wf != null)
                {
                    // check if functions match (there are probably some edge cases)
                    // TODO: add type matching for parameters (this could be hard because terraria references would need to match to objects)
                    if (patch.ReturnType != wf.ReturnType.FullName)
                    {
                        Output.Print($" > Return types did match \"{patch.ReturnType}\" != \"{wf.ReturnType.FullName}\".");
                        continue;
                    }
                    // check if param counts match
                    if (func.IsStatic && patch.ParameterCount != wf.GetParameters().Length)
                    {
                        Output.Print($" > Parameter count types did match, \"{patch.FunctionName}\" count: {patch.ParameterCount} \"{wf.Name}\" count: {wf.GetParameters().Length} (perameters need to match for patching static functions).");
                        continue;
                    }
                    else if (!func.IsStatic && patch.ParameterCount != (wf.GetParameters().Length - 1))
                    { // compare with - 1 to match instance peram
                        Output.Print($" > Parameter count types did match, \"{patch.FunctionName}\" count: {patch.ParameterCount} \"{wf.Name}\" count: {wf.GetParameters().Length} (\"{wf.Name}\" needs all the same parameters as \"{patch.FunctionName}\" plus an instance object as the first parameter).");
                        continue;
                    }
                    if (!funcPatchCount.ContainsKey(func)) funcPatchCount.Add(func, 0);
                    string originalName = patch.FunctionName + "_Original" + funcPatchCount[func]++;
                    patch.originalFunctionName = originalName;
                    var OriginalFunction = CloneMethod(moduleDef, func, originalName);
                    if (patch.ilModifications != null)
                    {
                        patch.ilModifications(OriginalFunction.Body, OriginalFunction.Body.GetILProcessor(), OriginalFunction.Body.Instructions, moduleDef);
                        Output.Print($" > Patched IL");
                    }

                    classType.Methods.Add(OriginalFunction);

                    var il = func.Body.GetILProcessor();
                    func.Body.ExceptionHandlers.Clear();
                    func.Body.Instructions.Clear();
                    if (!func.IsStatic)
                        il.Emit(OpCodes.Ldarg_0); // "this"
                    for (int i = 0; i < patch.ParameterCount; i++)
                        il.Emit(OpCodes.Ldarg, i + (func.IsStatic ? 0 : 1)); // add opp to push params

                    var wrapperFunction = moduleDef.ImportReference(wf);
                    il.Emit(OpCodes.Call, wrapperFunction);

                    il.Emit(OpCodes.Ret);

                    patch.Patched = true;
                    Output.Print($" > Complete (Original: \"{originalName}\")");
                }
                else if (patch.ilModifications != null)
                {
                    patch.ilModifications(func.Body, func.Body.GetILProcessor(), func.Body.Instructions, moduleDef);
                    Output.Print($" > Patched IL");
                    Output.Print($" > Complete \"{patch.FunctionName}\"");
                }
                else
                {
                    Output.Print($" > Why did you want to patch a function but then not add any patches.");
                }

            }

            // write patch into memory stream
            var ms = new MemoryStream();
            moduleDef.Write(ms);
            var patched = ms.ToArray();
            ms.Dispose();

            // output patched executable for debugging (this will not run on its own, the dll with the new function needs to also exist)
            // if (cModLoaderInitializer.PatchOutOverride || cModLoaderInitializer.Debug) // if VirtualLaunch don't create any file
            //     File.WriteAllBytes(realTerrariaPath.Substring(0, realTerrariaPath.Length - 4) + "_Patched.exe", patched);

            return patched; // return byte[] array because loading the dll here will recall AssemblyResolve causing a stack overflow

        }
        internal static void FinishPatch() {
            foreach (var patch in ModContent.patches) {
                if (patch.Patched) {
                    // ** using replace because Assembly.GetType() and ModuleDefinition.GetType() handle nested classes and structs differently
                    Type method = Terraria.TerrariaAsm.GetType(patch.ClassName.Replace('/', '+'));
                    if (method == null) {
                        throw new Exception($"Failed to find type \"{patch.ClassName.Replace("/", "+")}\"");
                    }
                    patch.originalFunction = method.GetMethod(patch.originalFunctionName); // no binding flags because it should be public
                }
            }
        }

        /// <summary>
        /// <para> This function is used to create a custom patch that allows you to modify default behavior of a Terraria function. (Will not do anything if patches are disabled) </para>
        /// <para> These patches only works on the Terraria assembly when it is loaded and can not be undone. </para>
        /// <para> I will use <c><see langword="public"/> <see langword="double"/> <see cref="Terraria"/>.<see cref="Player.Hurt"/></c> as an example as this function can not be patched using Harmony </para>
        /// <para> <b><paramref name="Class"/></b>: The class name that the function resides in. <u><b><i>eg.</i></b> This has to be the full name, instead of <c>"Player"</c> it would be <c>"Terraria.Player"</c>.</u> </para> 
        /// <para> <b><paramref name="FunctionName"/></b>: The name of function. <u><b><i>eg.</i></b> <c>"Hurt"</c>.</u> </para> 
        /// <para> <b><paramref name="ReturnType"/></b>: The return type of the function. <u><b><i>eg.</i></b> This has to be the full name, instead of <c>"double"</c> it would be <c>"System.Double"</c>.</u> </para> 
        /// <para> <b><paramref name="ParameterTypes"/></b>: The types of the function's parameters. <u><b><i>eg.</i></b> This has to be the full name, instead of <c>"double"</c> it would be <c>"System.Double"</c>.</u> </para> 
        /// <para> <b><paramref name="NewFunction"/></b>: The new function that will be ran instead, this needs reflection. <u><b><i>eg.</i></b>  <c>typeof(YourClass).GetMethod("HurtPatchExample")</c>.</u></para>
        /// <para>
        /// The patch function requires the same return type and parameters as the function you want to patch, plus an instance of the class if its not static as the first parameter.<br/>
        /// The patch function can NOT contain any reference to the terraria assembly in the parameters, for example <see cref="Player"/> or <see cref="PlayerDeathReason"/> would have to be changed to <see cref="object"/>.</para>
        /// <para> Example:<br/> <c><see langword="double"/> <see cref="HurtPatchExampleForNonStatic"/></c> for a non static function or <c><see langword="double"/> <see cref="HurtPatchExampleForStatic"/></c> for a static function </para> 
        /// <para> This overrides the original function and create a new functions with the original data, the original function can be called using <c><see cref="Patch.originalFunction"/></c> </para> 
        /// </summary>
        /// <returns> A <c><see cref="Patch"/></c> instance contain details about the patch, nothing can be modified but <c><see cref="Patch.originalFunction"/></c> can be used to obtain or call the original function. </returns>
        public static Patch AddPatch(string Class, string FunctionName, string ReturnType, string[] ParameterTypes, MethodInfo NewFunction)
            => new Patch(Class, FunctionName, ReturnType, ParameterTypes, NewFunction, null, !cModLoaderConfig.ForceDisablePatches);
        /// <summary>
        /// See <see cref="AddPatch(string, string, string, string[], MethodInfo)"/> for more information.
        /// <para> This is used to modify the IL code of the original function. This also creates a patch but changes the original function's code. </para>
        /// <para> Only use this if you know what your doing, no checks are done to make sure your modifications are valid. </para>
        /// </summary>
        public static Patch AddPatch(string Class, string FunctionName, string ReturnType, string[] ParameterTypes, MethodInfo NewFunction, ILModification ilModifications)
            => new Patch(Class, FunctionName, ReturnType, ParameterTypes, NewFunction, ilModifications, !cModLoaderConfig.ForceDisablePatches);

        internal static Patch ForcePatch(string Class, string FunctionName, string ReturnType, string[] ParameterTypes, MethodInfo NewFunction, ILModification ilModifications)
            => new Patch(Class, FunctionName, ReturnType, ParameterTypes, NewFunction, ilModifications, true);

        /// <summary> not ever called, just used for patch example </summary>
        public static double HurtPatchExampleForNonStatic(object __instance, object a, int b, int c, bool d, bool e, bool f, int g, bool h) { return 0.0; }
        /// <summary> not ever called, just used for patch example </summary>
        public static double HurtPatchExampleForStatic(object a, int b, int c, bool d, bool e, bool f, int g, bool h) { return 0.0; }
    }

}
