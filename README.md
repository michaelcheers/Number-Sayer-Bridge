# Number-Sayer-Bridge
A number sayer that uses Bridge.

I have made a Number Sayer in Bridge that has the following languages:
- English
- Esperanto
- French
- Spanish.
- German.

To try out, click [here](http://michael.cheersgames.com/Number%20Sayer/www).
You can try out the UI or if you want you can try out the api by pressing Ctrl+Shift+J and typing:
```javascript
new NumberSayer(NumberSayer.Language.English, "Michael").say$2('3').play()
```
or
```javascript
new NumberSayer(System.Enum.parse(NumberSayer.Language, 'English'), "Michael").say$2('3').play()
```
or
```javascript
new NumberSayer(0, "Michael").say$2('3').play()
```
NumberSayer.Language.English means English and Michael is the voice.

To use in a program (Windows):

1. Download Number Sayer Bridge as a zip by clicking Clone or Download->Download ZIP.
2. Extract All
3. Then open the folder it was extracted to if already open skip this step.
4. Double Click "Number-Sayer-Bridge-master"
5. Double Click "Number Sayer Bridge"
6. Highlight/Select the folder Bridge by single-clicking the tick box or the directory.
7. Press Ctrl+X and press Ctrl+V when you're in the location you want the API in.
8. Go to/Create your html file in side of the Bridge folder. (To create right click on in the Windows Explorer window and click New then Text Document and when choosing a name change the extension to html if you don't have extensions enabled create the file as a .txt then Press Windows+R and cmd then press Enter and type "rename " then drag the text file into the command prompt window and press space and then type index.html and press enter.
9. Add these to your index.html: 
```html
<script src="output/number_Sayer_Bridge.js"></script>
<script src="www/BigInteger.min.js"></script>
<script src="output/bridge.js"></script> 
```
and enjoy!

You can try:
```javascript
new NumberSayer().say$2('3').play()
```
