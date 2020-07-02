# CarrierJumpNotification
Tool for generating in-game chat notifications for Elite:Dangerous Fleet Carrier captains

[Download latest release HERE](https://github.com/SeaCrow/CarrierJumpNotification/releases)

Pulls data from Elite log and creates notification based on user set pattern.

![Tool Window](https://raw.githubusercontent.com/SeaCrow/CarrierJumpNotification/master/Misc/WindowImage.png)

Due to Elite:Dangerous weird handling of text pasting into chat (paste character limit) be mindful of notification length, there is option in Settings to cut out Col XXX Sector from system names to save on length. 
For example "Col 285 Sector CC-K a38-1" will be truncated in notification to "CC-K a38-1"

# Instructions
* Download latest release.
* Start the tool
* Schedule in-game FC jump.
* Click on "Data Pull" all fields should switch from example data to your informations.
If nothing happens either there is no FC jump scheduled in last 16min or tool can't find Elite Dangerous data.
In that second case, click on settings and set correct path to the Elite Dangerous save folder.
* If "Data Pull" was successful you should have brand new carrier jump notification ready to be pasted on the in-game System Chat

* If you want to publish another notification for the same jump sequence, just click "Generate Notification" to get new one with updated time remaining.

Fields above notification patterns are editable, if you want notification with some different text then system name (for example "Sell Area", "Col Mines"), just edit them and generate new notification.

Set notification pattern to whatever you want, full list of keywords is in Help. Just mind length for some bizzare reason in-game chat have paste character limit, too long notification wont paste correctly, so you might need to copy it in parts ( or shorten it).

### Buy me a beer
If you find tool useful and wish to buy me a beer, You can here: [Ko-Fi](https://ko-fi.com/qaazar#paymentModal)
