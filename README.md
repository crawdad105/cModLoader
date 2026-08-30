# ![cModLoader Logo](cModLoader/Logo.png)

cModLoader is a mod loader for most PC version of vanilla Terraria (on Windows and Linux, not MacOS yet).
It can be ran without making any patches to Terraria, ei. does not need MonoMod/Harmony, Mono.cecil or TerrariaInjector (Although it uses Mono.cecil but not necessarily for modifications).
<br><br>
cModLoader does NOT offer options to add items and stuff, its simply meant to give modders full and easy access to all of vanilla Terraria.
Its technically best for QoL mods or modifications to the Terraria engine.
If you want a way to add items and stuff either use tModLoader or for vanilla use TerrariaModder.
<br><br>
Using Mono.cecil there is a custom patcher that patches functions at an IL level which is the main selling point feature.
And can be used to override or modify the IL code of Terraria. (Mono.cecil is packaged with cModLoader as a resource, it is MIT licensed)
<br><br>
[cModLoader Web Page](https://crawdad105.com/cModLoader) (more info)<br>
[crawdad105 Website](https://crawdad105.com)<br>
[Discord Server](https://crawdad105.com/discord)<br>
Contact: Use whatever is convenient.
<br>

> [!IMPORTANT]
> Windows version 1.3.1 to 1.3.5.3 (inclusive) do not work, i think the issue is because the difference in framworks slightly changed how sorting works.<br>
> Linux only works in version 1.4.1 and up because i could not find hook location for older FNA versions.<br>
> I do not know how or if this works on servers, for now its not an implemented feature.<br>
> There may be versions that have issues, report them if you want them fixed.<br/>

> [!NOTE]
> This is not supposed to rivel tModLoader or TerrariaModder however if enough people want features like ways to add items, NPCs and more i may consider it. Its not necessarily hard to do but keep in mind i would need seamless integration for every versions of Terraria and to understand how every version of Terraria works on Windows and Linux (and maybe MaxOS in the future).

> [!CAUTION]
> "Raw Hooks" (hooks that do not require patches) are used for almost everything, its how this can exist without needed IL patches, however they are objectively less stable then normal IL parches as newer Terraria versions can break them and they rely heavily on a Xna or FNA, if those change they could break. If it becomes an issue i may consider switching to only using patches as they are safter and more stable.

MIT license because why not.
