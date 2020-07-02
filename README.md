# CarrierJumpNotification
Tool for generating in-game chat notifications for Elite:Dangerous Fleet Carrier captains

[Download latest release HERE](https://github.com/SeaCrow/CarrierJumpNotification/releases)

Pulls data from Elite log and creates notification based on user set pattern.

Due to the how data is saved to the logs, right now Tool can only figure out carrier pre-jump location if it was docked on the carrier in the current play session and before the jump.

If you didn't land on your carrier, after pulling data, manually correct "Jump from System" field and generate notification text again.

![Tool Window](https://raw.githubusercontent.com/SeaCrow/CarrierJumpNotification/master/Misc/WindowImage.png)

Due to Elite:Dangerous weird handling of text pasting into chat (paste character limit) be mindful of notification length, there is option in Settings to cut out Col XXX Sector from system names to save on length. 
For example "Col 285 Sector CC-K a38-1" will be truncated in notification to "CC-K a38-1"
