# Atem Karaoke
Windows application to lay text/lyrics over Blackmagic ATEM switcher's video. 

## Quick start
1. Download [AtemKaraoke.zip](https://github.com/EdMalashkin/AtemKaraoke/releases/latest) 
and unpack it to a folder. Your computer must be in the same local network as ATEM switcher is. Also, ATEM Software Control must be installed on the same computer.
2. Set these 3 settings both in AtemKaraoke.WinForm.exe.config and AtemKaraoke.Console.exe.config
  - SwitcherAddress
  - SourceFolder
  - DestinationFolder
3. Run AtemKaraoke.WinForm.exe
4. Paste some verses into the window. 
5. Press "Go To Live Mode" button to upload the verses to the switcher.
6. Press "Preview" button to output a verse to the Preview window of the switcher.
7. Press "On air" button to output the verse to the Program window of the switcher.
8. Press "Off air" button to stop outputting the verse.
