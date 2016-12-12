/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2016
 * @compiler Bridge.NET 15.3.0
 */
Bridge.assembly("Number Sayer Bridge", function ($asm, globals) {
    "use strict";

    Bridge.define("Number_Sayer_Bridge.Audio", {
        rnd: null,
        value: null,
        name: null,
        ctor: function (value, name, rnd) {
            if (name === void 0) { name = ""; }
            if (rnd === void 0) { rnd = null; }

            this.$initialize();
            this.value = value;
            this.rnd = rnd || new System.Random.ctor();
            this.name = name;
        },
        getaudio: function () {
            if (this.value.length === 0) {
                var error = System.String.concat("No valid audio for ", this.name, ".");
                Bridge.global.alert(error);
                throw new System.Exception(error);
            }
            return this.value[this.rnd.next$1(this.value.length)];
        }
    });

    Bridge.define("Number_Sayer_Bridge.BigDecimal", {
        statics: {
            parse: function (value) {
                var decPoints = value.split(String.fromCharCode(46));
                switch (decPoints.length) {
                    case 1: 
                        return new Number_Sayer_Bridge.BigDecimal(bigInt(value, 10), 0);
                    case 2: 
                        var b = decPoints[1];
                        return new Number_Sayer_Bridge.BigDecimal(bigInt(System.String.concat(decPoints[0], b), 10), decPoints[1].length);
                    default: 
                        throw new System.FormatException();
                }
            }
        },
        pow10Div: 0,
        config: {
            init: function () {
                this.value = new bigInt();
            }
        },
        ctor: function (value, pow10Div) {
            this.$initialize();
            this.value = value;
            this.pow10Div = pow10Div;
        },
        getPartA: function () {
            return this.value.over(bigInt(10).pow(this.pow10Div));
        },
        getPartB: function () {
            return this.value.mod(bigInt(10).pow(this.pow10Div)).abs();
        },
        N0s: function () {
            var $t;
            if (this.pow10Div === 0 || this.getPartB().eq(0)) {
                return bigInt(0);
            }
            var n0s = 0;
            $t = Bridge.getEnumerator(this.toString().split(String.fromCharCode(46))[1]);
            while ($t.moveNext()) {
                var item = $t.getCurrent();
                switch (item) {
                    case 48: 
                        n0s = (n0s + 1) | 0;
                        break;
                    case 45: 
                        break;
                    default: 
                        return bigInt(n0s);
                }
            }
            throw new System.Exception("Something bad happenned.");
        },
        toString: function () {
            var negative = this.value.lt(bigInt.zero);
            var vString = this.value.toString();
            if (negative) {
                vString = vString.substr(1);
            }
            if (this.pow10Div === 0) {
                return vString;
            }
            var insertLoc = (vString.length - this.pow10Div) | 0;
            if (insertLoc < 0) {
                var leftPiece = "";
                for (var n = 0; n > insertLoc; n = (n - 1) | 0) {
                    leftPiece = System.String.concat(leftPiece, "0");
                }
                vString = System.String.concat(leftPiece, vString);
                insertLoc = 0;
            }
            return System.String.insert(insertLoc, vString, System.String.concat(".", (negative ? "-" : "")));
        }
    });

    Bridge.define("NumberSayer", {
        statics: {
            knownVoices: null,
            irregularStarters: null,
            numberScale: null,
            placeValues: null,
            config: {
                init: function () {
                    this.knownVoices = $_.NumberSayer.f1(new (System.Collections.Generic.Dictionary$2(NumberSayer.Language,Array))());
                    this.irregularStarters = $_.NumberSayer.f2(new (System.Collections.Generic.Dictionary$2(NumberSayer.Language,System.Int32))());
                    this.numberScale = $_.NumberSayer.f3(new (System.Collections.Generic.Dictionary$2(NumberSayer.Language,System.Int32))());
                    this.placeValues = ["thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sedecillion", "septendecillion", "octodecillion", "novendecillion", "vigintillion"];
                }
            },
            /**
             * @static
             * @private
             * @this NumberSayer
             * @memberof NumberSayer
             * @param   {number}                               value
             * @return  {System.Collections.Generic.List$1}             The List: key|type, value|number of
             */
            romanNumeralization: function (value) {
                var result = new (System.Collections.Generic.List$1(System.Int32))(4);
                if (value === 9) {
                    value = (value - 9) | 0;
                    result.add(0);
                    result.add(2);
                }
                if (value >= 5) {
                    value = (value - 5) | 0;
                    result.add(1);
                }
                if (value === 4) {
                    result.add(0);
                    result.add(1);
                } else if (value !== 0) {
                    for (var n = 0; n < value; n = (n + 1) | 0) {
                        result.add(0);
                    }
                }
                return result;
            }
        },
        language: 0,
        smalls: null,
        voice: null,
        rnd: null,
        alreadyDone: null,
        config: {
            init: function () {
                this.rnd = new System.Random.ctor();
                this.alreadyDone = new (System.Collections.Generic.Dictionary$2(String,Number_Sayer_Bridge.Sound))();
            }
        },
        ctor: function (language, voice) {
            if (language === void 0) { language = 0; }
            if (voice === void 0) { voice = "Michael"; }

            this.$initialize();
            if (!System.Enum.isDefined(NumberSayer.Language, language)) {
                throw new System.ArgumentOutOfRangeException("language", "Value should be defined in the Language enum.");
            }
            this.language = language;
            this.voice = voice;
            switch (language) {
                case NumberSayer.Language.Esperanto: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9")];
                        break;
                    }
                case NumberSayer.Language.English: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12")];
                        break;
                    }
                case NumberSayer.Language.Spanish: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12"), this.loadSound("13"), this.loadSound("14"), this.loadSound("15")];
                        break;
                    }
                case NumberSayer.Language.French: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("1"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12"), this.loadSound("13"), this.loadSound("14"), this.loadSound("15"), this.loadSound("16")];
                        break;
                    }
                case NumberSayer.Language.German: 
                    {
                        this.smalls = [this.loadSound("0"), this.loadSound("eins"), this.loadSound("2"), this.loadSound("3"), this.loadSound("4"), this.loadSound("5"), this.loadSound("6"), this.loadSound("7"), this.loadSound("8"), this.loadSound("9"), this.loadSound("10"), this.loadSound("11"), this.loadSound("12")];
                        break;
                    }
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
            var $t, $t1, $t2;
            if (this.alreadyDone.containsKey(value)) {
                return this.alreadyDone.get(value);
            }
            var mixedResult = [];
            var format = System.String.concat("Sounds/", ($t=this.language, System.Enum.toString(NumberSayer.Language, $t)), "/{0}/{1}.wav");
            try {
                switch (this.voice) {
                    case "mixed": 
                        $t1 = Bridge.getEnumerator(NumberSayer.knownVoices.get(this.language));
                        while ($t1.moveNext()) {
                            var item = $t1.getCurrent();
                            mixedResult.push(this.createAudio(System.String.format(format, item, value), mixedResult));
                        }
                        break;
                    default: 
                        mixedResult.push(this.createAudio(System.String.format(format, this.voice, value), mixedResult));
                        break;
                }
            }
            catch ($e1) {
                $e1 = System.Exception.create($e1);
                if (Bridge.is($e1, System.Collections.Generic.KeyNotFoundException)) {
                    mixedResult.push(new Audio(System.String.format(format, "", "")));
                } else {
                    throw $e1;
                }
            }
            return (($t2 = new Number_Sayer_Bridge.Sound.$ctor1(new Number_Sayer_Bridge.Audio(mixedResult, value, this.rnd)), this.alreadyDone.set(value, $t2), $t2));
        },
        createAudio: function (value, mixedResult) {
            var result = new Audio();
            result.onerror = function (message, url, lineNumber, columnNumber, error) {
                var index = Bridge.Linq.Enumerable.from(mixedResult).indexOf(result);
                if (index > -1) {
                    mixedResult.splice(index, 1);
                }
                /*;
                return false; /// Unreachable code detected


                */
            };
            var source = Bridge.merge(document.createElement('source'), {
                src: value,
                onerror: result.onerror
            } );
            result.appendChild(source);
            return result;
        },
        getThirFifSound: function (value) {
            switch (value.toJSNumber()) {
                case 1: 
                    return this.loadSound("1");
                case 2: 
                    return this.loadSound("2");
                case 3: 
                    return this.language === NumberSayer.Language.English ? this.getthir() : this.loadSound("3");
                case 4: 
                    return this.loadSound("4");
                case 5: 
                    return this.language === NumberSayer.Language.English ? this.getfif() : this.loadSound("5");
                case 6: 
                    return this.loadSound("6");
                case 7: 
                    return this.loadSound(this.language === NumberSayer.Language.English ? "7" : "sieb");
                case 8: 
                    return this.loadSound(this.language === NumberSayer.Language.English ? "eigh" : "8");
                case 9: 
                    return this.loadSound("9");
            }
            throw new System.ArgumentException(value + " should only be 1 digit.");
        },
        say$1: function (value) {
            var $t;
            var result = new Number_Sayer_Bridge.Sound.ctor();
            if (this.language === NumberSayer.Language.Binary_Short) {
                var num = bigInt(1);
                var bit = 0;
                for (; num.lt(value); num = num.shiftLeft(1), bit = (bit + 1) | 0) {
                    ;
                }
                for (var currentNum = num; currentNum.neq(0); currentNum = currentNum.shiftRight(1), bit = (bit - 1) | 0) {
                    if ((value.and(currentNum)).eq(currentNum)) {
                        result.appendThis(this.sayBit(bit));
                    }
                }
                return result;
            }
            if (this.language !== NumberSayer.Language.Roman_Numerals) {
                if (value.lt(0)) {
                    return this.loadSound("minus").append(this.say$1(value.negate()));
                }
                if (value.lt(1000000)) {
                    if (value.lt(1000)) {
                        if (value.lt(100)) {
                            if (value.lt(20)) {
                                if (value.lt(NumberSayer.irregularStarters.get(this.language))) {
                                    result.appendThis(this.smalls[value.toJSNumber()]);
                                    return result;
                                }
                                switch (this.language) {
                                    case NumberSayer.Language.English: 
                                    case NumberSayer.Language.German: 
                                        {
                                            result.appendThis(this.getThirFifSound(value.mod(10)));
                                            result.appendThis(this.loadSound(this.language === NumberSayer.Language.German ? "10" : "teen"));
                                            return result;
                                        }
                                }
                            }
                            var dig1 = (value.over(10)).toJSNumber();
                            var dig2 = (value.mod(10)).toJSNumber();
                            switch (this.language) {
                                case NumberSayer.Language.English: 
                                case NumberSayer.Language.German: 
                                    {
                                        if (this.language === NumberSayer.Language.German && dig2 !== 0) {
                                            result.appendThis(this.getEinSound(dig2));
                                            result.appendThis(this.getand());
                                        }
                                        if (dig1 === 2) {
                                            result.appendThis(this.loadSound("20"));
                                        } else {
                                            result.appendThis(this.getThirFifSound(bigInt(dig1)));
                                            result.appendThis(this.getty());
                                        }
                                        if (this.language === NumberSayer.Language.German) {
                                            return result;
                                        }
                                        break;
                                    }
                                case NumberSayer.Language.Esperanto: 
                                    {
                                        if (dig1 !== 1) {
                                            result.appendThis(this.say$1(bigInt(dig1)));
                                        }
                                        result.appendThis(this.loadSound("10"));
                                        break;
                                    }
                                case NumberSayer.Language.Spanish: 
                                    {
                                        result.appendThis(this.loadSound(dig1 + "0"));
                                        if (dig2 !== 0) {
                                            result.appendThis(this.getand());
                                        }
                                        break;
                                    }
                                case NumberSayer.Language.French: 
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
                                                    if (dig220.neq(0)) {
                                                        result.appendThis(this.say$1(dig220));
                                                    }
                                                    return result;
                                                }
                                            default: 
                                                {
                                                    result.appendThis(this.loadSound(dig1 + "0"));
                                                    if (dig2 === 1) {
                                                        result.appendThis(this.getand());
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                            }
                            if (dig2 !== 0) {
                                result.appendThis(this.say$1(bigInt(dig2)));
                            }
                            return result;
                        }

                        var hundred = (value.over(100)).toJSNumber();
                        var remainder = (value.mod(100)).toJSNumber();
                        switch (this.language) {
                            case NumberSayer.Language.English: 
                            case NumberSayer.Language.German: 
                                {
                                    if (this.language === NumberSayer.Language.English || hundred !== 1) {
                                        result.appendThis(this.say$1(bigInt(hundred)));
                                    }
                                    result.appendThis(this.loadSound("hundred"));
                                    break;
                                }
                            case NumberSayer.Language.Spanish: 
                                {
                                    switch (hundred) {
                                        case 1: 
                                            {
                                                result.appendThis(remainder === 0 ? this.loadSound("100") : this.loadSound("ciento"));
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
                                                result.appendThis(this.say$1(bigInt(hundred)));
                                                result.appendThis(this.loadSound("hundred"));
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case NumberSayer.Language.French: 
                            case NumberSayer.Language.Esperanto: 
                                {
                                    switch (hundred) {
                                        case 1: 
                                            {
                                                result.appendThis(this.loadSound("hundred"));
                                                if (remainder === 1 && this.language === NumberSayer.Language.French) {
                                                    result.appendThis(this.loadSound("and"));
                                                }
                                                break;
                                            }
                                        default: 
                                            {
                                                result.appendThis(this.say$1(bigInt(hundred)));
                                                result.appendThis(this.loadSound("hundred"));
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        if (remainder !== 0) {
                            if (this.language === NumberSayer.Language.English) {
                                result.appendThis(this.getand());
                            }
                            result.appendThis(this.say$1(bigInt(remainder)));
                        }
                        ;
                        return result;
                    }
                    switch (this.language) {
                        case NumberSayer.Language.Spanish: 
                        case NumberSayer.Language.French: 
                        case NumberSayer.Language.Esperanto: 
                        case NumberSayer.Language.German: 
                            {
                                var part1 = value.over(1000);
                                var part2 = value.mod(1000);
                                if (part1.neq(1)) {
                                    result.appendThis(this.say$1(part1));
                                }

                                result.appendThis(this.loadSound("thousand"));
                                if (part2.neq(0)) {
                                    result.appendThis(this.say$1(part2));
                                }
                                return result;
                            }
                    }
                }
            } else if (value.lt(10)) {
                Bridge.Linq.Enumerable.from(NumberSayer.romanNumeralization(value.toJSNumber())).forEach(Bridge.fn.bind(this, function (v) {
                        result.appendThis(v === 2 ? this.loadSound("d_1_0") : this.loadSound("d_0_" + v));
                    }));
                return result;
            }
            var current = bigInt(1);
            var n = 0;
            var languageNumberScale = bigInt(NumberSayer.numberScale.get(this.language));
            for (; value.geq(current); n = (n + 1) | 0, current = current.times(languageNumberScale)) {
                ;
            }
            n = (n - 2) | 0;
            current = current.over(languageNumberScale);
            while (true) {
                var condition = n === -1;
                var currentVal = (value.over(current)).mod(languageNumberScale);
                if (currentVal.neq(0)) {
                    if (currentVal.lt(100) && condition && this.language === NumberSayer.Language.English) {
                        result.appendThis(this.getand());
                    }
                    var spanishAPart = (currentVal.over(1000)).toJSNumber();
                    var spanishBPart = (currentVal.mod(1000)).toJSNumber();
                    if (this.language === NumberSayer.Language.Roman_Numerals) {
                        var digits = NumberSayer.romanNumeralization(currentVal.toJSNumber());
                        $t = Bridge.getEnumerator(digits);
                        while ($t.moveNext()) {
                            var item = $t.getCurrent();
                            var lineNumbers = (Bridge.Int.div((((((n + 1) | 0) + ((item === 2) ? 1 : 0)) | 0)), 3)) | 0;
                            var append;
                            if (((n % 3) === 2 && item === 0) || (item === 2 && (n % 3) === 1)) {
                                lineNumbers = (lineNumbers - 1) | 0;
                                append = this.loadSound("d_3_0");
                            } else if (item === 2) {
                                append = this.loadSound("d_" + (((n + 2) | 0)) % 3 + "_0");
                            } else {
                                append = this.loadSound("d_" + (((n + 1) | 0)) % 3 + "_" + item);
                            }
                            result.appendThis(new Number_Sayer_Bridge.Sound.$ctor1(new Number_Sayer_Bridge.RomanNumeralsAudio(append.sound[0], lineNumbers)));
                            if (lineNumbers > 0) {
                                result.appendThis(this.loadSound("with"));
                                result.appendThis(this.say$1(bigInt(lineNumbers)));
                                result.appendThis(this.loadSound(lineNumbers === 1 ? "line" : "lines"));
                            }
                        }
                    } else {
                        result.appendThis((spanishBPart === 1 && !condition && this.language === NumberSayer.Language.Spanish) ? (spanishAPart === 0 ? new Number_Sayer_Bridge.Sound.ctor() : this.say$1(bigInt(((spanishAPart * 1000) | 0)))).append(this.loadSound("one")) : this.say$1(currentVal));
                    }
                    if (!condition) {
                        switch (this.language) {
                            case NumberSayer.Language.English: 
                                result.appendThis(this.loadSound(NumberSayer.placeValues[n]));
                                break;
                            case NumberSayer.Language.Spanish: 
                                result.appendThis(this.loadSound(NumberSayer.placeValues[((n + 1) | 0)]));
                                if (currentVal.neq(1)) {
                                    result.appendThis(this.loadSound("es"));
                                }
                                break;
                            case NumberSayer.Language.French: 
                            case NumberSayer.Language.Esperanto: 
                            case NumberSayer.Language.German: 
                                result.appendThis(this.loadSound(NumberSayer.placeValues[((Bridge.Int.div((((n + 1) | 0)), 2)) | 0)]).append(((((n + 1) | 0)) % 2) === 1 ? this.loadSound("ard") : this.loadSound("on")));
                                break;
                        }
                    }
                }
                current = current.over(languageNumberScale);
                var valMod1000000;
                if (current.eq(1000) && ((valMod1000000 = (value.mod(1000000)).toJSNumber())) !== 0 && this.language !== NumberSayer.Language.English && this.language !== NumberSayer.Language.Roman_Numerals) {
                    return result.append(this.say$1(bigInt(valMod1000000)));
                }
                n = (n - 1) | 0;
                if (current.eq(0)) {
                    return result;
                }
            }
        },
        say$2: function (value) {
            return this.say(Number_Sayer_Bridge.BigDecimal.parse(value));
        },
        say: function (value) {
            var $t, $t1;
            if (this.language === NumberSayer.Language.Roman_Numerals || this.language === NumberSayer.Language.Binary_Short) {
                if (value.pow10Div !== 0) {
                    throw new System.Exception("Decimals are invalid.");
                }
                if (value.value.leq(0)) {
                    throw new System.Exception("Negatives are invalid");
                }
                return this.say$1(value.value);
            }
            var s0s = new Number_Sayer_Bridge.Sound.ctor();
            for (var n = 0; bigInt(n).lt(value.N0s()); n = (n + 1) | 0) {
                s0s.appendThis(this.loadSound("0"));
            }
            var negative = value.value.lt(0) && value.getPartA().eq(0);
            switch (this.language) {
                case NumberSayer.Language.English: 
                    {
                        var partB = value.getPartB();
                        return (negative ? this.loadSound("minus") : new Number_Sayer_Bridge.Sound.ctor()).append(this.say$1(value.getPartA()).append(partB.eq(0) ? new Number_Sayer_Bridge.Sound.ctor() : this.loadSound("point").append(s0s).append(new Number_Sayer_Bridge.Sound.$ctor2(System.Array.convertAll(($t=partB.toString(), System.String.toCharArray($t, 0, $t.length)), Bridge.fn.bind(this, $_.NumberSayer.f4))))));
                    }
                case NumberSayer.Language.Spanish: 
                case NumberSayer.Language.French: 
                case NumberSayer.Language.German: 
                case NumberSayer.Language.Esperanto: 
                    {
                        var partB1 = value.getPartB();
                        return (negative ? this.loadSound("minus") : new Number_Sayer_Bridge.Sound.ctor()).append(this.say$1(value.getPartA()).append(partB1.eq(0) ? new Number_Sayer_Bridge.Sound.ctor() : this.loadSound("point").append(s0s).append(this.say$1(partB1))));
                    }
            }
            throw new System.NotImplementedException(System.String.concat("Unhandled language: ", ($t1=this.language, System.Enum.toString(NumberSayer.Language, $t1))));
        },
        sayBit: function (bit) {
            var div = (Bridge.Int.div(bit, 4)) | 0;
            if (div === 0) {
                return this.loadSound("bit_" + bit);
            } else {
                var mod = bit % 4;
                var sound = this.sayBit(mod);
                return this.sayBit(mod).append(this.loadSound("_start")).append(this.say$1(bigInt(div)).append(this.loadSound("_end")));
            }
        },
        getEinSound: function (dig2) {
            return dig2 === 1 ? this.loadSound("1") : this.say$1(bigInt(dig2));
        }
    });

    var $_ = {};

    Bridge.ns("NumberSayer", $_);

    Bridge.apply($_.NumberSayer, {
        f1: function (_o1) {
            _o1.add(NumberSayer.Language.English, ["Ally", "Ally (New)", "Ben (Silly)", "Erlantz", "Jeff", "Laurie", "Melissa", "Michael", "Pedro", "Seamus", "Sylvia"]);
            _o1.add(NumberSayer.Language.Spanish, ["Ana", "Sylvia"]);
            _o1.add(NumberSayer.Language.French, ["Ben", "Melissa"]);
            _o1.add(NumberSayer.Language.Esperanto, ["Michael"]);
            _o1.add(NumberSayer.Language.German, ["Ally", "Laurie", "Leire"]);
            _o1.add(NumberSayer.Language.Roman_Numerals, ["Michael"]);
            _o1.add(NumberSayer.Language.Binary_Short, ["Michael"]);
            return _o1;
        },
        f2: function (_o2) {
            _o2.add(NumberSayer.Language.English, 13);
            _o2.add(NumberSayer.Language.German, 13);
            _o2.add(NumberSayer.Language.Spanish, 16);
            _o2.add(NumberSayer.Language.French, 17);
            _o2.add(NumberSayer.Language.Esperanto, 10);
            return _o2;
        },
        f3: function (_o3) {
            _o3.add(NumberSayer.Language.English, 1000);
            _o3.add(NumberSayer.Language.French, 1000);
            _o3.add(NumberSayer.Language.German, 1000);
            _o3.add(NumberSayer.Language.Spanish, 1000000);
            _o3.add(NumberSayer.Language.Esperanto, 1000);
            _o3.add(NumberSayer.Language.Roman_Numerals, 10);
            return _o3;
        },
        f4: function (v) {
            return this.smalls[System.Int32.parse(String.fromCharCode(v))].sound[0];
        }
    });

    Bridge.define("NumberSayer.Language", {
        $kind: "enum",
        statics: {
            English: 0,
            Spanish: 1,
            French: 2,
            Esperanto: 3,
            German: 4,
            Roman_Numerals: 5,
            Binary_Short: 6
        }
    });

    Bridge.define("Number_Sayer_Bridge.Sound", {
        sound: null,
        config: {
            events: {
                OnEnded: null
            }
        },
        $ctor1: function (value) {
            this.$initialize();
            this.sound = [value];
        },
        $ctor2: function (value) {
            this.$initialize();
            this.sound = value;
        },
        ctor: function () {
            this.$initialize();
            this.sound = [];
        },
        play: function () {
            this.play$1($_.Number_Sayer_Bridge.Sound.f1);
        },
        play$1: function (callStart) {
            if (this.sound.length > 0) {
                this.play$2(0, callStart);
            }
        },
        play$2: function (index, callStart) {
            callStart(index);
            var audio = this.sound[index];
            var audioActual = audio.getaudio();
            if (this.sound.length !== ((index = (index + 1) | 0))) {
                audioActual.onended = Bridge.fn.bind(this, function (v) {
                    v.target.onended = $_.Number_Sayer_Bridge.Sound.f2;
                    this.play$2(index, callStart);
                });
            } else {
                audioActual.onended = Bridge.fn.bind(this, $_.Number_Sayer_Bridge.Sound.f3);
            }
            audioActual.play();
        },
        appendThis: function (sound) {
            this.sound = this.append(sound).sound;
        },
        append: function (sound) {
            var $t, $t1;
            var result = System.Array.init(((this.sound.length + sound.sound.length) | 0), null);
            ($t=this.sound, System.Array.copy($t, 0, result, 0, $t.length));
            ($t1=sound.sound, System.Array.copy($t1, 0, result, this.sound.length, $t1.length));
            return new Number_Sayer_Bridge.Sound.$ctor2(result);
        }
    });

    Bridge.ns("Number_Sayer_Bridge.Sound", $_);

    Bridge.apply($_.Number_Sayer_Bridge.Sound, {
        f1: function (v) {
        },
        f2: function (v2) {
        },
        f3: function (e) {
            !Bridge.staticEquals(this.OnEnded, null) ? this.OnEnded() : null;
        }
    });

    Bridge.define("Number_Sayer_Bridge.RomanNumeralsAudio", {
        inherits: [Number_Sayer_Bridge.Audio],
        lineNumbers: 0,
        ctor: function (value, lineNumbers) {
            this.$initialize();
            Number_Sayer_Bridge.Audio.ctor.call(this, value.value, value.name, value.rnd);
            this.lineNumbers = lineNumbers;
        }
    });
});
