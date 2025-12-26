# marrow-sdk-stand-ins

Storage for quality of life Marrow SDK scripts.

## Stand-Ins

| What                | Description                                                                                                                    | Prerequisites                                                                                          | Installation                           |
| ------------------- | ------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ | -------------------------------------- |
| Hot Packing         | Adds 'Hot Pack' utility for testing mods without restarting your game.                                                         | - [Insecticide](https://thunderstore.io/c/bonelab/p/Millzy/Insecticide/) (Patch 6 broke hot reloading) | Install modified `PalletEditor.cs`     |
| Level Launch Button | Adds 'Launch' button to level crates that opens BONELAB                                                                        | - LevelLoader.dll (needs publishing)                                                                   | Install modified `LevelCrateEditor.cs` |
| Value Node Fix      | Exposes the ValueNode getter and removes the exception. Useful for storing floats in UltEvents.                                |                                                                                                        | Install modified `ValueCircuit.cs`     |
| Dev Console Utils   | Adds `Stand Ins -> Take Me Here` button under the context menu that appears when right clicking Game Objects in the hierarchy. |                                                                                                        | Install `DevConsoleUtils.cs`           |