using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static cModLoader.Terraria;
using Microsoft.CodeAnalysis.Semantics;
using System.Runtime.InteropServices;

namespace cModLoader.Utils {

    /// <summary> Extensions. </summary>
    public static class Extensions {
        /// <summary> Converts this to cModLoader's <see cref="Dynamic"/> type.</summary>
        public static Dynamic AsDynamic(this object obj) => new Dynamic(obj);
        /// <summary> Create an instance of <paramref name="type"/>.</summary>
        public static object CreateInstance(this Type type) => Activator.CreateInstance(type);
        /// <summary> Create a <see cref="Dynamic"/> instance of <paramref name="type"/>.</summary>
        public static Dynamic CreateDynamicInstance(this Type type) => new Dynamic(Activator.CreateInstance(type));
        /// <summary> Create a <see cref="Dynamic"/> instance of <paramref name="type"/> with <paramref name="parameters"/>.</summary>
        public static Dynamic CreateDynamicInstance(this Type type, params object[] parameters) => new Dynamic(Activator.CreateInstance(type, parameters));
    }

    /// <summary>
    /// Reference to game stuff, used almost everywhere so things can easily be added without messing with previous references.
    /// </summary>
    public struct GameReference {
        /// <summary> Static reference for <see cref="GameReference"/>, if you have access to a local one use that, this is a last resort.</summary>
        public static GameReference StaticReference;
        /// <summary> Creates </summary>        
        internal GameReference(Game g) {
            main = new Dynamic(g);
            game = g;
            // Terraria.Main should always contain a "spriteBatch" variables, this could be an issue in the future if the decide to remove it
            spriteBatch = main.GetValue<SpriteBatch>("spriteBatch");
            gameTime = main.GetValue<GameTime>("gameTime");
            StaticReference = this;
        }
        /// <summary> Terraria's main instance. </summary>
        public Dynamic main;
        /// <summary> Terraria's main but as its inheriting <see cref="Game"/> type. </summary>
        public Game game;
        /// <summary> <see cref="SpriteBatch"/> used for rendering. </summary>
        public SpriteBatch spriteBatch;
        /// <summary> <see cref="GameTime"/> used for game time stuff. </summary>
        public GameTime gameTime;

    }

    /// <summary>
    /// Wrapper for Relogic's Asset&lt;T&gt; class where <see cref="Asset"/> is the asset and <see cref="Value"/> is the "Value" of the asset.<br/>
    /// In versions where this does not exist (See <see cref="VersionChecks.Using_RelogicAssets"/>) both <see cref="Asset"/> and <see cref="Value"/> are the same thing with type <typeparamref name="T"/>.
    /// <para>
    /// This is not used to create new instances of Asset&lt;T&gt;, for that see <see cref="Relogic.Asset{T}(string)"/> or <see cref="Relogic.HackAsset{T}(T, string)"/>
    /// </para>
    /// </summary>
    public struct ReLogicAsset<T> where T : class {
        private bool IsAsset;
        public ReLogicAsset(object Asset) {
            if (Asset.GetType().FullName.StartsWith("cModLoader.Utils.ReLogicAsset`1")) {
                Output.Print("An instance of ReLogicAsset was created with another instance of ReLogicAsset. This is likely a bug so it is managed. But fix your code.");
                // we should really just not allow the user to do this but who cares.
                // we need reflection here because we cant cast
                var oldDynamic = (Dynamic)Asset.GetType().GetField("DynamicReference", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Asset);
                Asset = oldDynamic.Value;
            }
            this.DynamicReference = new Dynamic(Asset);
            IsAsset = Asset.GetType().FullName.StartsWith("ReLogic.Content.Asset`1");
        }
        /// <summary> This <see cref="Dynamic"/> instance, use <see cref="Asset"/> or <see cref="Value"/> to get related values.</summary>
        public Dynamic DynamicReference;
        /// <summary> if <see cref="VersionChecks.Using_RelogicAssets"/> then its type Asset&lt;T&gt; otherwise its <typeparamref name="T"/>. </summary>
        public object Asset => DynamicReference.Value;
        /// <summary> Always <typeparamref name="T"/>, can be used to safely obtain the value. </summary>
        public T Value => IsAsset ? DynamicReference.GetValue<T>("Value") : (T)DynamicReference.Value;
        public static explicit operator T(ReLogicAsset<T> asset) => asset.Value;
        public static explicit operator ReLogicAsset<T>(T raw) => new ReLogicAsset<T>(raw);
    }

    /// <summary>
    /// Used instead of <see langword="dynamic"/> because Linux cant seem to handle loading it in some contexts.<br/>
    /// This is also faster then <see langword="dynamic"/> and reflection as it creates delegates for and caches getters, setters and methods.
    /// </summary>
    public class Dynamic {

        private const int cacheSizeIncrement = 32;
        private class MethodRef {
            public Func<object, object[], object> foo = null;
            public bool isStatic = false;
        }
        private class CacheData {
            public CacheData(Type type) {
                Type = type;
            }
            public Type Type;
            public Dictionary<string, Func<object, object>> Field_Getters = new Dictionary<string, Func<object, object>>();
            public Dictionary<string, Action<object, object>> Field_Setters = new Dictionary<string, Action<object, object>>();
            public Dictionary<string, MethodRef> Methods = new Dictionary<string, MethodRef>();
            public void Destroy() {
                Type = null;
                Field_Getters.Clear();
                Field_Getters = null;
                Field_Setters.Clear();
                Field_Setters = null;
                Methods.Clear();
                Methods = null;
            }
        }

        private static Dictionary<Type, CacheData> cacheData = new Dictionary<Type, CacheData>(cacheSizeIncrement);
        private static Dictionary<Type, int> instances = new Dictionary<Type, int>(cacheSizeIncrement);

        private CacheData currentData;
        /// <summary>
        /// The object this instance is representing.<br/>
        /// You can set this to a new value to retain type information without needing to recompute anything but make sure its the same type.
        /// </summary>
        public object Value;
        /// <summary> The type of the object this instance is representing or the static type provided. </summary>
        public Type ValueType => currentData == null ? null : currentData.Type;
        /// <summary> Used for accessing methods and fields form an static type. </summary>
        public Dynamic(Type staticType) {
            if (staticType == null) {
                throw new Exception("An instance of Dynamic was created with a null staticType parameter value. This is a big. Fix your code.");
            }
            Value = null;
            if (!cacheData.TryGetValue(staticType, out currentData)) {
                currentData = new CacheData(staticType);
                cacheData.Add(staticType, currentData);
            }
            int num = 0;
            if (instances.TryGetValue(staticType, out num)) { }
            instances[staticType] = num + 1;
        }
        /// <summary> Used for accessing methods and fields form an instance of an object. </summary>
        public Dynamic(object value) {
            if (value == null) {
                // "This is likely a bug", but i want it to not be...
                // Output.Print("An instance of dynamic was created with a null object. This is likely a bug so it is managed. But fix your code.");
                Value = null;
                currentData = null;
                return;
            }
            Value = value;
            var type = value.GetType();
            if (value is Dynamic d) {
                Output.Print("An instance of Dynamic was created with another instance of Dynamic. This is likely a bug so it is managed. But fix your code.");
                // we should really just not allow the user to do this but who cares.
                Value = d.Value;
                type = d.Value.GetType();
            }
            if (!cacheData.TryGetValue(type, out currentData)) {
                currentData = new CacheData(type);
                cacheData.Add(type, currentData);
            }
            int num = 0;
            if (instances.TryGetValue(type, out num)) { }
            instances[type] = num + 1;
        }

        // Finalize()
        ~Dynamic() {
            if (ValueType != null && instances.TryGetValue(ValueType, out int num)) {
                instances[ValueType] = Math.Min(num - 1, 0);
            }
        }

        /// <summary> Natively calls a function with a given name.</summary>
        public T Invoke<T>(string functionName, params object[] parameters) => (T)Invoke(true, functionName, parameters);
        /// <summary> Natively calls a function with a given name.</summary>
        public void Invoke(string functionName, params object[] parameters) => Invoke(false, functionName, parameters);
        /// <summary> Natively calls a function with a given name. (each element in <paramref name="parameters"/> is a parameter)<para>Does not handle functions with same names, use <see cref="OverrideCachedMethod(string, Type[], MethodInfo)"/> to customize invocation. </para></summary>
        public T Invoke2<T>(string functionName, object[] parameters) => (T)Invoke(true, functionName, parameters);
        /// <summary> Natively calls a function with a given name. (each element in <paramref name="parameters"/> is a parameter)<para>Does not handle functions with same names, use <see cref="OverrideCachedMethod(string, Type[], MethodInfo)"/> to customize invocation. </para></summary>
        public void Invoke2(string functionName, object[] parameters) => Invoke(false, functionName, parameters);
        private object Invoke(bool isReturn, string functionName, object[] parameters) {
            if (currentData == null) throw new Exception("Dynamic Invoke can not be called with a null type.");
            Type[] types = parameters.Select(x => x.GetType()).ToArray();
            string functionID = functionName + "," + string.Join(",", types.Select(x => x.FullName));
            if (!currentData.Methods.TryGetValue(functionID, out var foo)) {
                if (foo == null) foo = new MethodRef();
                MethodInfo method = null;
                var allMethods = GetAllMethods(ValueType);
                // get matching functions
                MethodInfo[] foundMethods = allMethods.Where(x => x.Name == functionName && x.GetParameters().Length == parameters.Length).ToArray();
                if (foundMethods.Length == 0) {
                    throw new Exception($"Dynamic Invoke failed to find method with name \"{functionName}\" and {parameters.Length} parameters.");
                } else if (foundMethods.Length > 1) {
                    if (parameters.Length > 0) {
                        // check matching parameters
                        int[] matches = new int[foundMethods.Length];
                        for (int i = 0; i < foundMethods.Length; i++) {
                            var para = foundMethods[i].GetParameters();
                            for (int j = 0; j < para.Length; j++) {
                                // exact match, TODO: add better checks for inheritance
                                if (para[j].GetType() == parameters[j].GetType()) {
                                    matches[i]++;
                                }
                            }                        
                        }
                        // get best matches 
                        var largest = -1; // negative one so even if the max is 0 it still gives a functions
                        var best = 0;
                        for (int i = 0; i < matches.Length; i++) {
                            if (matches[i] > largest) { // 'greater then' only so this just gets the first, if they are equal, then we get issues
                                largest = matches[i];
                                best = i;
                            }
                        }
                        // just get the first, otherwise we would need to re-run this whole thing again with better parameter checks
                        method = foundMethods[best];
                    } else { // no parameters to check
                        // we could check return type but 2 functions with the same signature (excluding return) cant exist
                        throw new Exception($"Dynamic Invoke found {foundMethods.Length} matching methods with name \"{functionName}\" and no parameters, this shouldn't be possible.");
                    }

                } else { // only 1 found
                    method = foundMethods[0];
                }
                foo.foo = CreateFunction(method, functionName);
                foo.isStatic = method.IsStatic;
                currentData.Methods.Add(functionID, foo);
            }
            if (isReturn) return foo.foo(Value, parameters);
            foo.foo(Value, parameters);
            return null;
        }
        /// <summary> Natively gets a field or property.<br/>Supports nested values, Eg. <c>"Rect.Size.Width"</c> is valid.</summary>
        public T GetValue<T>(string fieldName) {
            if (currentData == null) throw new Exception("Dynamic GetValue can not be called on a null object.");
            var values = fieldName.Split('.');
            if (!currentData.Field_Getters.TryGetValue(values[0], out var foo)) {
                foo = CreateGetter(ValueType, values[0]);
                currentData.Field_Getters.Add(values[0], foo);
            }
            if (values.Length == 1) return (T)foo(Value);
            else {
                return new Dynamic(foo(Value)).GetValue<T>(fieldName.Substring(fieldName.IndexOf(".") + 1));
            }
        }
        /// <summary> Natively sets a field or property to <paramref name="newValue"/>.<br/>Supports nested values, Eg. <c>"Rect.Size.Width"</c> is valid.<br/>Returns <paramref name="newValue"/>.</summary>
        public T SetValue<T>(string fieldName, T newValue) {
            if (currentData == null) throw new Exception("Dynamic SetValue can not be called on a null object.");
            var values = fieldName.Split('.');
            if (!currentData.Field_Setters.TryGetValue(values[0], out var foo)) {
                foo = CreateSetter(ValueType, values[0]);
                currentData.Field_Setters.Add(values[0], foo);
            }
            if (values.Length == 1) foo(Value, newValue);
            else {
                (new Dynamic(GetValue<object>(fieldName))).SetValue(fieldName.Substring(fieldName.IndexOf(".") + 1), newValue);
            }
            return newValue;
        }

        /// <summary> Allows you to override what method is cached<br/>When <see cref="Invoke(string, object[])"/> or <see cref="Invoke{T}(string, object[])"/> is call <paramref name="overrideMethod"/> will be called instead.<br/>This will not do anything to original vanilla functionality.</summary>
        public void OverrideCachedMethod(string functionName, Type[] types, MethodInfo overrideMethod) {
            if (currentData == null) throw new Exception("Dynamic OverrideCachedMethod can not be called on a null object.");
            string functionID = functionName + "," + string.Join(",", types.Select(x => x.FullName));
            if (!currentData.Methods.TryGetValue(functionName, out var foo)) {
                if (foo == null) foo = new MethodRef();
                foo.isStatic = overrideMethod.IsStatic;
                foo.foo = CreateFunction(overrideMethod, functionName);
                currentData.Methods.Add(functionID, foo);
            }
            else {
                currentData.Methods[functionID] = foo;
            }
        }

        // cant do with this because dynamic is object
        // public static explicit operator dynamic(object raw) => new dynamic(raw);

        private static List<FieldInfo> GetAllFields(Type type) {
            var fields = new List<FieldInfo>();
            fields.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static));
            if (type.BaseType != null) {
                fields.AddRange(GetAllFields(type.BaseType));
            }
            return fields;
        }
        private static List<PropertyInfo> GetAllProperties(Type type) {
            var fields = new List<PropertyInfo>();
            fields.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static));
            if (type.BaseType != null) {
                fields.AddRange(GetAllProperties(type.BaseType));
            }
            return fields;
        }
        private static List<MethodInfo> GetAllMethods(Type type) {
            var fields = new List<MethodInfo>();
            fields.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static));
            if (type.BaseType != null) {
                var newMethods = GetAllMethods(type.BaseType);
                foreach (var item in newMethods) {
                    // only get new methods, skip overriding ones, this should give only the highest level ones
                    if (fields.Where(x
                        // check name
                        => x.Name == item.Name &&
                        // check "signature"
                        string.Join(",", x.GetParameters().Select(y => y.ParameterType.FullName)) == string.Join(",", item.GetParameters().Select(y => y.ParameterType.FullName))
                        ).Count() == 0) {
                        // add if no matches
                        fields.Add(item);
                    }
                }
            }
            return fields;
        }

        // Claude helped with this
        /// <summary> Creates lambda expression that natively calls a given function. </summary>
        private static Func<object, object[], object> CreateFunction(MethodInfo method, string funcName) {
            if (method == null) throw new Exception($"Dynamic failed to create function. method was null for function \"{funcName}\". Check your function name and parameter types.");
            // create parameters
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var argsParam =     Expression.Parameter(typeof(object[]), "args");
            // create arguments, casts args to the there respective types
            var parameters = method.GetParameters();
            var paramsExpressions = new Expression[parameters.Length];
            var variables = new List<ParameterExpression>(); // local variables
            var preCall = new List<Expression>(); // casts from args[] to local
            var postCall = new List<Expression>(); // casts from local to args[]

            for (int i = 0; i < parameters.Length; i++) {
                var argsIndex = Expression.ArrayAccess(argsParam, Expression.Constant(i)); // args[i]
                var paramType = parameters[i].ParameterType;
                if (paramType.IsByRef) { // check if "ByRef"
                    // create local variable
                    var elementType = paramType.GetElementType();
                    var localVar = Expression.Variable(elementType, "arg" + i); // create variable by name "argN"
                    variables.Add(localVar);
                    // check if its a "ref" instead of "out", for "ref" we dont need to read any value
                    if (!parameters[i].IsOut)
                        preCall.Add(Expression.Assign(localVar, Expression.Convert(argsIndex, elementType))); // local = (T)args[i]
                    postCall.Add(Expression.Assign(argsIndex, Expression.Convert(localVar, typeof(object)))); // args[i] = (object)local
                    paramsExpressions[i] = localVar; // put local var into params
                }
                else {
                    paramsExpressions[i] = Expression.Convert(argsIndex, parameters[i].ParameterType); // cast args[i] to parameter type. (T)args[i]
                }
            }
            // create call, check for static
            Expression call = method.IsStatic ? Expression.Call(method, paramsExpressions) : Expression.Call(Expression.Convert(instanceParam, method.DeclaringType), method, paramsExpressions);
            // create body, check for return
            var body = new List<Expression>(preCall); // start body with casts to locals.

            if (method.ReturnType == typeof(void)) {
                body.Add(call); // add call
                body.AddRange(postCall); // add "ref" and "out" conversions back into arg[i] // args[i] = (object)local
                body.Add(Expression.Constant(null, typeof(object))); // add final result, this is null because no return value
            }
            else {
                var resultVar = Expression.Variable(typeof(object), "result"); // create result variable
                variables.Add(resultVar);
                body.Add(Expression.Assign(resultVar, Expression.Convert(call, typeof(object)))); // add call and set new variable // result = (object)call
                body.AddRange(postCall); // add "ref" and "out" conversions back into arg[i] // args[i] = (object)local
                body.Add(resultVar); // add final result, this returns "result"
            }

            // create lambda expression
            var lambda = Expression.Lambda<Func<object, object[], object>>(Expression.Block(variables, body), instanceParam, argsParam);
            return lambda.Compile();
        }
        /// <summary> Creates lambda expression that natively gets a value. </summary>
        private static Func<object, object> CreateGetter(Type type, string fieldName) {
            // create perameters
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            // create cast, not used for static things
            var cast = Expression.Convert(instanceParam, type);
            MemberExpression memberExpression = null;

            var field = GetAllFields(type).Where(x => x.Name == fieldName).ToArray();
            if (field == null || field.Length == 0) {
                var prop = GetAllProperties(type).Where(x => x.Name == fieldName).ToArray();
                if (prop == null || prop.Length == 0) throw new Exception($"Dynamic failed to create getter. Both field and property were null for field \"{fieldName}\" on type \"{type.FullName}\". Check your field name.");
                bool isStatic = (prop[0].CanRead && prop[0].GetMethod.IsStatic) || (prop[0].CanWrite && prop[0].SetMethod.IsStatic);
                memberExpression = Expression.Property(isStatic ? null : cast, prop[0]);
            } else {
                memberExpression = Expression.Field(field[0].IsStatic ? null : cast, field[0]);
            }
            // create body and lambda expression
            var body = Expression.Convert(memberExpression, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(body, instanceParam);
            return lambda.Compile();
        }
        /// <summary> Creates lambda expression that natively sets a value. </summary>
        private static Action<object, object> CreateSetter(Type type, string fieldName) {
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var valueParam = Expression.Parameter(typeof(object), "value");
            var cast = Expression.Convert(instanceParam, type);

            MemberExpression memberExpression = null;
            Type fieldType = null;

            var field = GetAllFields(type).Where(x => x.Name == fieldName).ToArray();
            if (field == null || field.Length == 0) {
                var prop = GetAllProperties(type).Where(x => x.Name == fieldName).ToArray();
                if (prop == null || prop.Length == 0) throw new Exception($"Dynamic failed to create setter. Both field and property were null for field \"{fieldName}\" on type \"{type.FullName}\". Check your field name.");
                // cant set these
                if (!prop[0].CanWrite) return (a, b) => {
                    Output.Error($"Tried to set property \"{fieldName}\" but it was read only.");
                };
                memberExpression = Expression.Property(cast, prop[0]);
                fieldType = prop[0].PropertyType;
            }
            else {
                // cant set these
                if (field[0].IsInitOnly || field[0].IsLiteral) return (a, b) => { 
                    Output.Error($"Tried to set field \"{fieldName}\" but it was read only."); 
                };
                memberExpression = Expression.Field(field[0].IsStatic ? null : cast, field[0]);
                fieldType = field[0].FieldType;
            }

            var assign = Expression.Assign(memberExpression, Expression.Convert(valueParam, fieldType));
            var lambda = Expression.Lambda<Action<object, object>>(assign, instanceParam, valueParam);
            return lambda.Compile();
        }

        /// <summary>
        /// Removes cache data and calls <see cref="GC.Collect()"/>, this may not actually do anything if there are <see cref="Dynamic"/> instances that exist.<br/>
        /// Does not clean base types.
        /// </summary>        
        public static void CleanType(Type type) {
            if (instances.TryGetValue(type, out int num) && num <= 0) {
                cacheData[type].Destroy();
                instances.Remove(type);
                GC.Collect();
            }
        }
        /// <summary>
        /// Returns the number of <see cref="Dynamic"/> instances exist with <paramref name="type"/>.<br/>Returns -1 if <paramref name="type"/> was not cached.<br/>
        /// </summary>        
        public static int InstanceCount(Type type) {
            if (instances.TryGetValue(type, out int num)) {
                return num;
            }
            return -1;
        }

    }



}
