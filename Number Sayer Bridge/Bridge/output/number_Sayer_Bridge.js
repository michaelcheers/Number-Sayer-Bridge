(function (globals) {
    "use strict";

    Bridge.define('Number_Sayer_Bridge.Audio', {
        rnd: null,
        value: null,
        name: null,
        constructor: function (value, name, rnd) {
            if (name === void 0) { name = ""; }
            if (rnd === void 0) { rnd = null; }
    
            this.value = value;
            this.rnd = rnd == null ? new Bridge.Random("constructor") : rnd;
            this.name = name;
        },
        getaudio: function () {
            return this.value[this.rnd.next$1(this.value.length)];
        }
    });
    
    Bridge.define('Number_Sayer_Bridge.NumberSayer', {
        statics: {
            knownVoices: null,
            irregularStarters: null,
            numberScale: null,
            placeValues: null,
            config: {
                init: function () {
                    this.knownVoices = Bridge.merge(new Bridge.Dictionary$2(Number_Sayer_Bridge.NumberSayer.Language,Array)(), [
        [Number_Sayer_Bridge.NumberSayer.Language.English, ["Ally", "Ben", "Jeff", "Laurie", "Melissa", "Michael", "Seamus"]],
        [Number_Sayer_Bridge.NumberSayer.Language.Spanish, ["Ana", "Sylvia"]],
        [Number_Sayer_Bridge.NumberSayer.Language.French, ["Ben"]],
        [Number_Sayer_Bridge.NumberSayer.Language.Esperanto, ["Michael"]]
    ] );
                    this.irregularStarters = Bridge.merge(new Bridge.Dictionary$2(Number_Sayer_Bridge.NumberSayer.Language,Bridge.Int32)(), [
        [Number_Sayer_Bridge.NumberSayer.Language.English, 13],
        [Number_Sayer_Bridge.NumberSayer.Language.Spanish, 16],
        [Number_Sayer_Bridge.NumberSayer.Language.French, 17],
        [Number_Sayer_Bridge.NumberSayer.Language.Esperanto, 10]
    ] );
                    this.numberScale = Bridge.merge(new Bridge.Dictionary$2(Number_Sayer_Bridge.NumberSayer.Language,Bridge.Int32)(), [
        [Number_Sayer_Bridge.NumberSayer.Language.English, 1000],
        [Number_Sayer_Bridge.NumberSayer.Language.French, 1000],
        [Number_Sayer_Bridge.NumberSayer.Language.Spanish, 1000000],
        [Number_Sayer_Bridge.NumberSayer.Language.Esperanto, 1000]
    ] );
                    this.placeValues = ["thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sedecillion", "septendecillion", "octodecillion", "novendecillion", "vigintillion"];
                }
            }
        },
        language: 0,
        smalls: null,
        voice: null,
        rnd: null,
        alreadyDone: null,
        config: {
            init: function () {
                this.rnd = new Bridge.Random("constructor");
                this.alreadyDone = new Bridge.Dictionary$2(String,Number_Sayer_Bridge.Sound)();
            }
        },
        constructor: function (language, voice) {
            if (language === void 0) { language = 0; }
            if (voice === void 0) { voice = "Michael"; }
    
            this.language = language;
            this.voice = voice;
            switch (language) {
                case Number_Sayer_Bridge.NumberSayer.Language.Esperanto: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9")];
                        break;
                    }
                case Number_Sayer_Bridge.NumberSayer.Language.English: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12")];
                        break;
                    }
                case Number_Sayer_Bridge.NumberSayer.Language.Spanish: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12"), this.loadSound("13"), this.loadSound("14"), this.loadSound("15")];
                        break;
                    }
                case Number_Sayer_Bridge.NumberSayer.Language.French: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12"), this.loadSound("13"), this.loadSound("14"), this.loadSound("15"), this.loadSound("16")];
                        break;
                    }
                default: 
                    break;
            }
        },
        getthir: function () {
            return this.loadSound("thir");
        },
        getfif: function () {
            return this.loadSound("fif");
        },
        getand: function () {
            return this.loadSound("and");
        },
        getty: function () {
            return this.loadSound("ty");
        },
        loadSound: function (value) {
            var $t;
            if (this.alreadyDone.containsKey(value)) {
                return this.alreadyDone.get(value);
            }
            var mixedResult = [];
            var format = "Sounds/" + Bridge.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, this.language) + "/{0}/{1}.wav";
            try {
                if (Bridge.referenceEquals(this.voice, "mixed")) {
                    $t = Bridge.getEnumerator(Number_Sayer_Bridge.NumberSayer.knownVoices.get(this.language));
                    while ($t.moveNext()) {
                        var item = $t.getCurrent();
                        mixedResult.push(new Audio(Bridge.String.format(format, item, value)));
                    }
                }
                else  {
                    mixedResult.push(new Audio(Bridge.String.format(format, this.voice, value)));
                }
            }
            catch ($e1) {
                $e1 = Bridge.Exception.create($e1);
                var e;
                if (Bridge.is($e1, Bridge.KeyNotFoundException)) {
                    e = $e1;
                    mixedResult.push(new Audio(Bridge.String.format(format, "", "")));
                }
                else {
                    throw $e1;
                }
            }
            return ((this.alreadyDone.set(value, new Number_Sayer_Bridge.Sound("constructor$1", new Number_Sayer_Bridge.Audio(mixedResult, value, this.rnd))), this.alreadyDone.get(value)));
        },
        getThirFifSound: function (value) {
            switch (value.toJSNumber()) {
                case 1: 
                    return this.loadSound("1");
                case 2: 
                    return this.loadSound("2");
                case 3: 
                    return this.getthir();
                case 4: 
                    return this.loadSound("4");
                case 5: 
                    return this.getfif();
                case 6: 
                    return this.loadSound("6");
                case 7: 
                    return this.loadSound("7");
                case 8: 
                    return this.loadSound("8");
                case 9: 
                    return this.loadSound("9");
            }
            throw new Bridge.ArgumentException(value + " should only be 1 digit.");
        },
        say: function (value) {
            var result = new Number_Sayer_Bridge.Sound("constructor");
            if (value.lt(1000000)) {
                if (value.lt(1000)) {
                    if (value.lt(100)) {
                        if (value.lt(20)) {
                            if (value.lt(Number_Sayer_Bridge.NumberSayer.irregularStarters.get(this.language))) {
                                result.appendThis(this.smalls[value.toJSNumber()]);
                                return result;
                            }
                            switch (this.language) {
                                case Number_Sayer_Bridge.NumberSayer.Language.English: 
                                    {
                                        result.appendThis(this.getThirFifSound(value.mod(10)));
                                        result.appendThis(this.loadSound("teen"));
                                        return result;
                                    }
                                default: 
                                    break;
                            }
                        }
                        var dig1 = value.over(10);
                        var dig2 = value.mod(10);
                        switch (this.language) {
                            case Number_Sayer_Bridge.NumberSayer.Language.English: 
                                {
                                    if (dig1.eq(2)) {
                                        result.appendThis(this.loadSound("20"));
                                    }
                                    else  {
                                        result.appendThis(this.getThirFifSound(dig1));
                                        result.appendThis(this.getty());
                                    }
                                    break;
                                }
                            case Number_Sayer_Bridge.NumberSayer.Language.Esperanto: 
                                {
                                    if (dig1.neq(1)) {
                                        result.appendThis(this.say(dig1));
                                    }
                                    result.appendThis(this.loadSound("10"));
                                    break;
                                }
                            case Number_Sayer_Bridge.NumberSayer.Language.Spanish: 
                                {
                                    result.appendThis(this.loadSound(dig1 + "0"));
                                    if (dig2.neq(0)) {
                                        result.appendThis(this.getand());
                                    }
                                    break;
                                }
                            case Number_Sayer_Bridge.NumberSayer.Language.French: 
                                {
                                    var dig120 = (Bridge.Int.div(value.toJSNumber(), 20)) | 0;
                                    var dig220 = value.mod(20);
                                    switch (dig120) {
                                        case 3: 
                                        case 4: 
                                            {
                                                result.appendThis(this.loadSound(((dig120 * 2) | 0) + "0"));
                                                if (dig120 === 3 && dig220.eq(1)) {
                                                    result.appendThis(this.getand());
                                                }
                                                result.appendThis(this.say(dig220));
                                                return result;
                                            }
                                        default: 
                                            {
                                                result.appendThis(this.loadSound(dig1 + "0"));
                                                if (dig2.eq(1)) {
                                                    result.appendThis(this.getand());
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            default: 
                                break;
                        }
                        if (dig2.neq(0)) {
                            result.appendThis(this.say(dig2));
                        }
                        return result;
                    }
    
                    var hundred = (value.over(100)).toJSNumber();
                    var remainder = value.mod(100);
                    switch (this.language) {
                        case Number_Sayer_Bridge.NumberSayer.Language.English: 
                            {
                                result.appendThis(this.say(bigInt(hundred)));
                                result.appendThis(this.loadSound("hundred"));
                                break;
                            }
                        case Number_Sayer_Bridge.NumberSayer.Language.Spanish: 
                            {
                                switch (hundred) {
                                    case 1: 
                                        {
                                            if (remainder.eq(0)) {
                                                result.appendThis(this.loadSound("100"));
                                            }
                                            else  {
                                                result.appendThis(this.loadSound("ciento"));
                                            }
                                            break;
                                        }
                                    case 5: 
                                        {
                                            result.appendThis(this.loadSound("500"));
                                            break;
                                        }
                                    case 7: 
                                        {
                                            result.appendThis(this.loadSound("700"));
                                            break;
                                        }
                                    case 9: 
                                        {
                                            result.appendThis(this.loadSound("900"));
                                            break;
                                        }
                                    default: 
                                        {
                                            result.appendThis(this.say(bigInt(hundred)));
                                            result.appendThis(this.loadSound("hundred"));
                                            break;
                                        }
                                }
                                break;
                            }
                        case Number_Sayer_Bridge.NumberSayer.Language.French: 
                        case Number_Sayer_Bridge.NumberSayer.Language.Esperanto: 
                            {
                                switch (hundred) {
                                    case 1: 
                                        {
                                            result.appendThis(this.loadSound("hundred"));
                                            if (remainder.eq(1) && this.language === Number_Sayer_Bridge.NumberSayer.Language.French) {
                                                result.appendThis(this.loadSound("and"));
                                            }
                                            break;
                                        }
                                    default: 
                                        {
                                            result.appendThis(this.say(bigInt(hundred)));
                                            result.appendThis(this.loadSound("hundred"));
                                            break;
                                        }
                                }
                                break;
                            }
                        default: 
                            break;
                    }
                    if (remainder.neq(0)) {
                        if (this.language === Number_Sayer_Bridge.NumberSayer.Language.English) {
                            result.appendThis(this.getand());
                        }
                        result.appendThis(this.say(remainder));
                    }
                    ;
                    return result;
                }
                switch (this.language) {
                    case Number_Sayer_Bridge.NumberSayer.Language.Spanish: 
                    case Number_Sayer_Bridge.NumberSayer.Language.French: 
                    case Number_Sayer_Bridge.NumberSayer.Language.Esperanto: 
                        {
                            var part1 = value.over(1000);
                            var part2 = value.mod(1000);
                            if (part1.neq(1)) {
                                result.appendThis(this.say(part1));
                            }
                            result.appendThis(this.loadSound("thousand"));
                            if (part2.neq(0)) {
                                result.appendThis(this.say(part2));
                            }
                            return result;
                        }
                    default: 
                        break;
                }
            }
            var current = bigInt(1);
            var n = 0;
            var languageNumberScale = bigInt(Number_Sayer_Bridge.NumberSayer.numberScale.get(this.language));
            for (; value.greaterOrEquals(current); n = (n + 1) | 0, current = current.times(languageNumberScale)) {
                ;
            }
            n = (n - 2) | 0;
            current = current.over(languageNumberScale);
            while (true) {
                var condition = n === -1;
                var currentVal = (value.over(current)).mod(languageNumberScale);
                if (currentVal.neq(0)) {
                    if (currentVal.lt(100) && condition && this.language === Number_Sayer_Bridge.NumberSayer.Language.English) {
                        result.appendThis(this.getand());
                    }
                    if (currentVal.neq(1) || this.language === Number_Sayer_Bridge.NumberSayer.Language.English) {
                        result.appendThis((currentVal.eq(1) && this.language === Number_Sayer_Bridge.NumberSayer.Language.Spanish) ? this.loadSound("one") : this.say(currentVal));
                    }
                    if (!condition) {
                        switch (this.language) {
                            case Number_Sayer_Bridge.NumberSayer.Language.English: 
                                result.appendThis(this.loadSound(Number_Sayer_Bridge.NumberSayer.placeValues[n]));
                                break;
                            case Number_Sayer_Bridge.NumberSayer.Language.Spanish: 
                                result.appendThis(this.loadSound(Number_Sayer_Bridge.NumberSayer.placeValues[((n + 1) | 0)]));
                                if (currentVal.neq(1)) {
                                    result.appendThis(this.loadSound("es"));
                                }
                                break;
                            case Number_Sayer_Bridge.NumberSayer.Language.French: 
                            case Number_Sayer_Bridge.NumberSayer.Language.Esperanto: 
                                result.appendThis(this.loadSound(Number_Sayer_Bridge.NumberSayer.placeValues[((Bridge.Int.div((((n + 1) | 0)), 2)) | 0)]).append(((((n + 1) | 0)) % 2) === 1 ? this.loadSound("ard") : this.loadSound("on")));
                                break;
                            default: 
                                break;
                        }
                    }
                }
                current = current.over(languageNumberScale);
                var valMod1000000;
                if (current.eq(1000) && ((valMod1000000 = (value.mod(1000000)).toJSNumber())) !== 0 && this.language !== Number_Sayer_Bridge.NumberSayer.Language.English) {
                    return result.append(this.say(bigInt(valMod1000000)));
                }
                n = (n - 1) | 0;
                if (current.eq(0)) {
                    return result;
                }
            }
        }
    });
    
    Bridge.define('Number_Sayer_Bridge.NumberSayer.Language', {
        statics: {
            English: 0,
            Spanish: 1,
            French: 2,
            Esperanto: 3
        },
        $enum: true
    });
    
    Bridge.define('Number_Sayer_Bridge.Sound', {
        sound: null,
        constructor$1: function (value) {
            this.sound = [value];
        },
        constructor$2: function (value) {
            this.sound = value;
        },
        constructor: function () {
            this.sound = [];
        },
        play: function () {
            if (this.sound.length > 0) {
                this.play$1(0);
            }
        },
        play$1: function (index) {
            var audio = this.sound[index];
            var audioActual = audio.getaudio();
            if (this.sound.length !== ((index = (index + 1) | 0))) {
                audioActual.onended = Bridge.fn.bind(this, function (v) {
                    v.target.onended = $_.Number_Sayer_Bridge.Sound.f1;
                    this.play$1(index);
                });
            }
            audioActual.play();
        },
        appendThis: function (sound) {
            this.sound = this.append(sound).sound;
        },
        append: function (sound) {
            var result = Bridge.Array.init(((this.sound.length + sound.sound.length) | 0), null);
            Bridge.Array.copy(this.sound, 0, result, 0, this.sound.length);
            Bridge.Array.copy(sound.sound, 0, result, this.sound.length, sound.sound.length);
            return new Number_Sayer_Bridge.Sound("constructor$2", result);
        }
    });
    
    var $_ = {};
    
    Bridge.ns("Number_Sayer_Bridge.Sound", $_)
    
    Bridge.apply($_.Number_Sayer_Bridge.Sound, {
        f1: function (v2) {
        }
    });
    
    Bridge.init();
})(this);
