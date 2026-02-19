
# Coming soon (Hopefully)
# cModLoader (Crawdad's mod loader)

cModLoader is a mod loader for vanilla Terraria allowing the user to mod vanilla Terraria. The purpose of cModLoader is to give vanilla Terraria players like my self the option to create mods for it. It was made so i could test very niche mechanics in vanilla Terraria without worrying about change between vanilla and tModLoader. It was not intended to make large mods like Calamity but rather for smaller engine mods such as reweighting the rendering system to rendering 100,000s of blocks [\[Video not out yet\]](https://www.youtube.com/@crawdad105). Although with the way progression is going it might just be good for larger mods.
Personally i don't like playing modded, i want a vanilla experience, i have over 9,300 hours in vanilla Terrariaand only 2,200 in tModLoader. So i made cModLoader to give me access to the inner working of Terraria without going through any tModLoader changes.

### "What's your problem with tModLoader"
Because tModLoader is technically very different from Terraria, many very niche game mechanics could have been changes or fixed for tModLoader or where tModLoader introduces bugs that don't exist in the vanilla game. The original reason i create cModLoader was so i could test these things. It isn't just trying to be tModLoader, in fact, i am purposely leaving out many features that tModLoader has so its not associated with it. My goal was to create a mod loader not bound by the limitations or changes of tModLoader. cModLoader was created to give modders to the entire game with no limitations, if you want to change anything just do it. tModLoader already has a lot you can modify, I've been told everything cModLoader claims to do can already be done in tModLoader, but cModLoader was build with more then just smaller game modifications in mind.

## Versions support and OS
cModLoader, when finished, should work on any versions of Terraria on any platform (idk about macOS though, i have no way of testing for it). This allows you to creates mods for even the leaked 0.1 version. Originally it was going to stay only working with the newest versions but after 1.4.5 came out it would be helpful if i wanted to play older versions. Aside from getting working on any versions i want it to work on at least Windows and Linux. it was primary developed for Windows but after realizing, it shouldn't be too hard to port to Linux. Unfortunately because i don't own and never plan on owning an apple product cModLoader will likely not work on macOS but who knows maybe in the future ill add that.

## How it works
Unlike other vanilla Terraria mods and mod loaders it does not redistribute a cracked or modified versions of the game. This works completely by it self assuming you have Terraria installed. It works by taking the place of the Terraria exe so when you launch the game from steam or wherever it starts cModLoader which then launches Terraria . When cModLoader is open it will automatically load any mods or patches for Terraria . When Terraria is being loaded it will apply those patches in the form of IL modifications (which doesn't require you to write any IL code) then loads the changed EXE. The patches are all done ahead of time, this means there is no runtime patching required which could cause problems. It uses Mono.Cecil to do IL modifications and the patching is a custom implementation which does not rely on any external libraries such as Harmoney or MonoMod (other then Mono.Cecil, which is packaged as a resource). Allowing IL modifications means you can programmatically add fields, classes and functions into Terraria. This allows for native modifications and at least in 1.4.5 allows you to add items just by adding 1 field instead of modifying every reference of the Item.Count. in theory you could create an entirely new game just by creating a bunch of IL patches requiring only your DLL mod, in fact if you do it well enough you could export the EXE and create a cracked or modified Terraria EXE that doesn't rely on anything to run, but i do not condone this and it should only be used for testing or debugging.

## How do mods work?
**This does NOT and will likely NOT run tModLoader mods.**
There are 2 ways mods can be loaded, either form loading it after Terraria has started using the "Upload Mod DLL" or playing it in a folder that cModLoader generates for the mods. Loading the mod after the games has started could be probably through because patches will not work. Developing a mod is as easy as creating a c# project using .NET Framework 4.5.2 and adding cModLoader as a reference. Then you can create a mod class which will automatically be loaded when your DLL is loaded. Obviously because its not out you have no way of doing this at the moment. 

# Development
cModLoader is in active development, currently only 1 person has a very early beta build.
It currently works in versions 0.1 to 1.0.6.1 on windows (im working my way up) but i want to get it working on Linux before adding any other features. 
Adding Linux support shouldn't be too hard, it works fine with Win but Terraria runs using Steam's Proton which means i need it to work with Proton which has different features. For example System.Windows.Froms works fine in Wine but Proton does not support it, although Terraria is shipped with it, its barely uses, they actually use a custom implementation of MessageBox.Show using SDL3. Because i want cModLoader to work flawlessly everywhere on every versions i need to implement a way to get a Windows form like form in Linux without using windows form using SDL3 because that's shipped with Terraria. I also need it to work with the same code as windows forms so the modder can create a window without writing a bunch of other code for 2 different platforms.

Full Support: ✅<br>
Partial Support: ☑️<br>
Not Tested: ✴️ Not tested by likely works<br>
In Development: 🛠️<br>
No Support: ❌ (Currently)<br>
IDK if this versions exists: ❔(I'm too lazy to find out)<br>
Versions Probably Doesn't exist: ❓(that aren't public available)<br>
Out Of Date: ➖ Version didn't exist officially on this platform<br>
Steam Version: s Versions downloadable through steam<br>
Hotfix Versions numbers: <sup>n</sup> Versions that are likely hotfixes that are slightly different but the same version<br>

| Versions | Windows | Linux | macOS |
|:-------- |:------: | :---: | :----: |
| Support  | ✅ | 🛠️ | ❌ |
| |  |  |  |
| 0.1 | ☑️ | ➖ | ➖ |
| 0.2 - 0.6 | ❓ | ➖ | ➖ |
| 0.7 | ✅ | ➖ | ➖ |
| 0.8 - 0.9 | ❓ | ➖ | ➖ |
| 1.0 | ✅ | ➖ | ➖ |
| 1.0.1 | ✅ | ➖ | ➖ |
| 1.0.2 | ✅ | ➖ | ➖ |
| 1.0.3 | ✅ | ➖ | ➖ |
| 1.0.4 | ✅ | ➖ | ➖ |
| 1.0.5 | ✅ | ➖ | ➖ |
| 1.0.6 | ✅ | ➖ | ➖ |
| 1.0.6.1 | ✅ | ➖ | ➖ |
| s1.0.6.1 | ✴️ | 🛠️ | ❌ |
| |  |  |  |
| 1.4.4.9<sup>1</sup>* | ✴️ | ❌ | ❌ |
| 1.4.4.9<sup>2</sup>* | ✴️ | ❔ | ❔ |
| 1.4.4.9<sup>3</sup>* | ✅ | ❔ | ❔ |
| 1.4.5* | ✅ | ❌ | ❌ |
| 1.4.5.1* | ✅ | ❌ | ❌ |
| 1.4.5.2* | ✅ | ❌ | ❌ |
| 1.4.5.3* | ✅ | ❌ | ❌ |
| 1.4.5.4* | ✅ | ❌ | ❌ |
| 1.4.5.5* | ✅ | ❌ | ❌ |


\* Working in older versions, but might not in the newer verions
