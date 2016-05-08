(function (globals) {
    "use strict";

    Bridge.define('Number_Sayer_Bridge.HTML', {
        statics: {
            sayers: null,
            config: {
                init: function () {
                    this.sayers = new Bridge.Dictionary$2(String,Number_Sayer_Bridge.NumberSayer)();
                    Bridge.ready(this.start);
                }
            },
            getvoice: function () {
                return document.getElementById("voice");
            },
            getlanguage: function () {
                return document.getElementById("language");
            },
            getcurrentLanguage: function () {
                return Number_Sayer_Bridge.HTML.getlanguage().selectedIndex;
            },
            getcurrentVoice: function () {
                return Number_Sayer_Bridge.HTML.getvoice().value;
            },
            getsaid: function () {
                return document.getElementById("said");
            },
            start: function () {
                var $t;
                document.getElementById("submit").onclick = Number_Sayer_Bridge.HTML.submit;
                $t = Bridge.getEnumerator(Bridge.Enum.getValues(Number_Sayer_Bridge.NumberSayer.Language));
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    Number_Sayer_Bridge.HTML.getlanguage().appendChild(Bridge.merge(document.createElement('option'), {
                        value: Bridge.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, item),
                        innerHTML: Bridge.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, item)
                    } ));
                }
                Number_Sayer_Bridge.HTML.getlanguage().selectedIndex = 0;
                Number_Sayer_Bridge.HTML.update();
            },
            submit: function (arg) {
                var key = Number_Sayer_Bridge.HTML.getcurrentVoice() + Bridge.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, Number_Sayer_Bridge.HTML.getcurrentLanguage());
                var sayer;
                if (Number_Sayer_Bridge.HTML.sayers.containsKey(key)) {
                    sayer = Number_Sayer_Bridge.HTML.sayers.get(key);
                }
                else  {
                    sayer = ((Number_Sayer_Bridge.HTML.sayers.set(key, new Number_Sayer_Bridge.NumberSayer(Number_Sayer_Bridge.HTML.getcurrentLanguage(), Number_Sayer_Bridge.HTML.getcurrentVoice())), Number_Sayer_Bridge.HTML.sayers.get(key)));
                }
                var sound = sayer.say(new bigInt(document.getElementById("number").value));
                sound.play();
                var saidString = new Bridge.List$1(String)();
                Bridge.Linq.Enumerable.from(sound.sound).forEach(function (v) {
                    saidString.add(v.name);
                });
                Number_Sayer_Bridge.HTML.getsaid().innerHTML = Bridge.String.replaceAll(Bridge.String.replaceAll(Bridge.String.replaceAll(saidString.join(" "), " es", "es"), " ty", "ty"), " teen", "teen");
            },
            update: function () {
                var $t;
                Number_Sayer_Bridge.HTML.getvoice().innerHTML = "";
                var currentKnownVoices = Number_Sayer_Bridge.NumberSayer.knownVoices.get(Number_Sayer_Bridge.HTML.getlanguage().selectedIndex);
                $t = Bridge.getEnumerator(currentKnownVoices);
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    Number_Sayer_Bridge.HTML.getvoice().appendChild(Bridge.merge(document.createElement('option'), {
                        value: item.toString(),
                        innerHTML: item.toString()
                    } ));
                }
                Number_Sayer_Bridge.HTML.getvoice().appendChild(Bridge.merge(document.createElement('option'), {
                    value: "mixed",
                    innerHTML: "Mixed"
                } ));
            }
        }
    });
    
    
    
    Bridge.init();
})(this);
