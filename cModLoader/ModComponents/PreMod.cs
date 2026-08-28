using cModLoader.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cModLoader.ModComponents
{
    /// <summary>
    /// <para>Pre-mod for doing things before you mod is loaded override <see cref="RegisterMod()"/> to register your mod.</para>
    /// <para>You can NOT reference the terraria assembily in this class or else cModLoader will crash.</para>
    /// </summary>
    public abstract class PreMod {
        internal Assembly assembly = null;
        internal Mod modReference = null;
        internal List<cModPatch.Patch> modPatches = new List<cModPatch.Patch>();

        /// <summary> Used so the mod loader doesn't break when trying to patch on the wrong versions. Set this in the contractor. </summary>
        public List<Version> SupportedVersions = new List<Version>();
        /// <summary>
        /// This is the first thing ran after the constructor dictating whether or not this mod should be loaded.<br/>If this returns <see langword="true"/> the mod will continue to load, if <see langword="false"/> it will NOT continue and will be discarded.<br/>By default it checks <see cref="SupportedVersions"/> however you can override and change it.
        /// <para> Technically you could run some code here and return <see langword="false"/> to do things like patch Terraria then not load a pre-mod. </para>
        /// </summary>
        public virtual bool ValidVersion() {
            return SupportedVersions.Contains(Terraria.GameVersion);
        }
        /// <summary>
        /// <para>Called when an instance of this class is created. This is before Terraria is loaded. </para>
        /// <para>Add patches through <see cref="Patching.cModPatch.AddPatch(string, string, string, string[], System.Reflection.MethodInfo)"/><br/>or run other code here before Terraria is loaded.</para>
        /// </summary>
        public virtual void OnLoad() { }
        /// <summary>
        /// <para>
        /// <paramref name="Windows"/>: Called on the second update tick of Terraria before anything happens.
        /// </para>
        /// <para>
        /// <paramref name="Linux"/>: Called on the first tick of Terraria before anything happens.
        /// </para>
        /// </summary>
        public virtual void OnStart() { }
        /// <summary> 
        /// This is where your mod is loaded. This will end up calling <see cref="Mod.OnInitialize()"/>.<br/>
        /// By default this returns <see langword="null"/>, if you want to do anything meaningly you should override it.
        /// <para>
        /// Called directly after <see cref="OnStart"/>
        /// </para>
        /// </summary>
        public virtual Mod RegisterMod() {
            return null;
        }
    }
}
