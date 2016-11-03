/// <reference path="Bridge/output/number_Sayer_Bridge.js" />
/// <reference path="Bridge/output/bridge.js" />

test("Say 1000000", function ()
{
   var sayer = new NumberSayer();
   var sound = sayer.say$2("1000000").sound.map(function(v){return v.name});
   equal(sound[0], "1");
});
