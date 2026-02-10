using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manages the 30-lesson structured curriculum
/// </summary>
public class LessonManager
{
    private readonly List<LessonInfo> _lessons;

    public LessonManager()
    {
        _lessons = InitializeLessons();
    }

    /// <summary>
    /// Get all lessons
    /// </summary>
    public List<LessonInfo> GetAllLessons()
    {
        return _lessons;
    }

    /// <summary>
    /// Get lessons by category
    /// </summary>
    public List<LessonInfo> GetLessonsByCategory(LessonCategory category)
    {
        return _lessons.Where(l => l.Category == category).ToList();
    }

    /// <summary>
    /// Get lesson by number
    /// </summary>
    public LessonInfo? GetLessonByNumber(int lessonNumber)
    {
        return _lessons.FirstOrDefault(l => l.LessonNumber == lessonNumber);
    }

    /// <summary>
    /// Get next lesson
    /// </summary>
    public LessonInfo? GetNextLesson(int currentLessonNumber)
    {
        return _lessons.FirstOrDefault(l => l.LessonNumber == currentLessonNumber + 1);
    }

    /// <summary>
    /// Initialize all 30 lessons
    /// </summary>
    private List<LessonInfo> InitializeLessons()
    {
        return new List<LessonInfo>
        {
            // HOME ROW (Lessons 1-5)
            new LessonInfo
            {
                LessonNumber = 1,
                Category = LessonCategory.HomeRow,
                Title = "বেসিক হোম রো - মধ্যম সারি",
                Description = "কিবোর্ডের মধ্যম সারির অক্ষর শিখুন। আঙুলের প্রাথমিক অবস্থান।",
                FocusKeys = "a s d f j k l ;",
                TextContent = "asdf jkl; asdf jkl; asdf jkl; fdsa ;lkj fdsa ;lkj",
                Difficulty = "Beginner",
                EstimatedMinutes = 5
            },
            new LessonInfo
            {
                LessonNumber = 2,
                Category = LessonCategory.HomeRow,
                Title = "হোম রো - বাংলা স্বরবর্ণ",
                Description = "বাংলা স্বরবর্ণ (অ, আ, ই, ঈ, উ, ঊ) প্র্যাকটিস করুন।",
                FocusKeys = "a s d f g j k l",
                TextContent = "অ অ অ আ আ আ ই ই ই ঈ ঈ ঈ উ উ উ ঊ ঊ ঊ",
                Difficulty = "Beginner",
                EstimatedMinutes = 6
            },
            new LessonInfo
            {
                LessonNumber = 3,
                Category = LessonCategory.HomeRow,
                Title = "হোম রো - মৌলিক ব্যঞ্জনবর্ণ",
                Description = "ক, খ, গ, ঘ, চ, ছ, জ, ঝ লিখতে শিখুন।",
                FocusKeys = "j k l g h d f s",
                TextContent = "ক ক ক খ খ খ গ গ গ ঘ ঘ ঘ চ চ চ ছ ছ ছ জ জ জ ঝ ঝ ঝ",
                Difficulty = "Beginner",
                EstimatedMinutes = 7
            },
            new LessonInfo
            {
                LessonNumber = 4,
                Category = LessonCategory.HomeRow,
                Title = "হোম রো - সহজ শব্দ",
                Description = "হোম রো অক্ষর দিয়ে গঠিত সহজ শব্দ প্র্যাকটিস করুন।",
                FocusKeys = "all home row",
                TextContent = "কাক খাক গাল ছাদ জাল আজ কাজ খাজা গাছ ছাগ",
                Difficulty = "Beginner",
                EstimatedMinutes = 8
            },
            new LessonInfo
            {
                LessonNumber = 5,
                Category = LessonCategory.HomeRow,
                Title = "হোম রো - বাক্য",
                Description = "হোম রো অক্ষর দিয়ে সহজ বাক্য লিখুন।",
                FocusKeys = "all home row",
                TextContent = "আজ আকাশ ভাল। কাক খাদ্য খাচ্ছ। গাছ ছাগল খাচ্ছ।",
                Difficulty = "Beginner",
                EstimatedMinutes = 10
            },

            // TOP ROW (Lessons 6-10)
            new LessonInfo
            {
                LessonNumber = 6,
                Category = LessonCategory.TopRow,
                Title = "উপরের সারি - পরিচিতি",
                Description = "কিবোর্ডের উপরের সারির অক্ষর শিখুন।",
                FocusKeys = "q w e r t y u i o p",
                TextContent = "qwert yuiop qwert yuiop trewq poiuy trewq poiuy",
                Difficulty = "Beginner",
                EstimatedMinutes = 6
            },
            new LessonInfo
            {
                LessonNumber = 7,
                Category = LessonCategory.TopRow,
                Title = "উপরের সারি - ব্যঞ্জনবর্ণ",
                Description = "ট, ঠ, ড, ঢ, ণ, ত, থ, দ, ধ, ন অক্ষর প্র্যাকটিস করুন।",
                FocusKeys = "t y u i o p",
                TextContent = "ট ট ট ঠ ঠ ঠ ড ড ড ঢ ঢ ঢ ণ ণ ণ ত ত ত থ থ থ দ দ দ ধ ধ ধ ন ন ন",
                Difficulty = "Intermediate",
                EstimatedMinutes = 7
            },
            new LessonInfo
            {
                LessonNumber = 8,
                Category = LessonCategory.TopRow,
                Title = "হোম + উপর সারি - শব্দ",
                Description = "হোম রো এবং উপরের সারির সমন্বয়ে শব্দ তৈরি করুন।",
                FocusKeys = "home + top row",
                TextContent = "টাকা ঠাকুর ডাক ঢাকা তাল থালা দাম ধান নাক পাত",
                Difficulty = "Intermediate",
                EstimatedMinutes = 8
            },
            new LessonInfo
            {
                LessonNumber = 9,
                Category = LessonCategory.TopRow,
                Title = "উপর সারি - বাক্য গঠন",
                Description = "উপরের সারি ব্যবহার করে সহজ বাক্য লিখুন।",
                FocusKeys = "home + top row",
                TextContent = "ঠাকুর নদীতে নৌকা দেখছেন। ডাক্তার ডাকুন। তাল গাছ ঢাকায়।",
                Difficulty = "Intermediate",
                EstimatedMinutes = 10
            },
            new LessonInfo
            {
                LessonNumber = 10,
                Category = LessonCategory.TopRow,
                Title = "উপর সারি - দ্রুততা বৃদ্ধি",
                Description = "উপরের সারির দক্ষতা বাড়ান এবং গতি বৃদ্ধি করুন।",
                FocusKeys = "home + top row",
                TextContent = "পাতা নাটক তারা দিন টাকা পথ থাক ইতি নতুন পুরাতন দেশ থেকে তিনি",
                Difficulty = "Intermediate",
                EstimatedMinutes = 12
            },

            // BOTTOM ROW (Lessons 11-15)
            new LessonInfo
            {
                LessonNumber = 11,
                Category = LessonCategory.BottomRow,
                Title = "নিচের সারি - পরিচিতি",
                Description = "কিবোর্ডের নিচের সারির অক্ষর শিখুন।",
                FocusKeys = "z x c v b n m",
                TextContent = "zxcvb nm zxcvb nm vbnmx zcvbn vbnmx zcvbn",
                Difficulty = "Intermediate",
                EstimatedMinutes = 6
            },
            new LessonInfo
            {
                LessonNumber = 12,
                Category = LessonCategory.BottomRow,
                Title = "নিচের সারি - ব্যঞ্জনবর্ণ",
                Description = "প, ফ, ব, ভ, ম, য, র, ল, শ, ষ, স, হ প্র্যাকটিস করুন।",
                FocusKeys = "z x c v b n m",
                TextContent = "প প প ফ ফ ফ ব ব ব ভ ভ ভ ম ম ম য য য র র র ল ল ল শ শ শ ষ ষ ষ স স স হ হ হ",
                Difficulty = "Intermediate",
                EstimatedMinutes = 8
            },
            new LessonInfo
            {
                LessonNumber = 13,
                Category = LessonCategory.BottomRow,
                Title = "সম্পূর্ণ কিবোর্ড - শব্দ",
                Description = "সব সারি মিলিয়ে শব্দ গঠন করুন।",
                FocusKeys = "all rows",
                TextContent = "পাখি ফুল বই ভাত মাছ যাও রাত লাল শহর সাথে হাত",
                Difficulty = "Advanced",
                EstimatedMinutes = 10
            },
            new LessonInfo
            {
                LessonNumber = 14,
                Category = LessonCategory.BottomRow,
                Title = "নিচের সারি - বাক্য",
                Description = "সম্পূর্ণ কিবোর্ড ব্যবহার করে বাক্য লিখুন।",
                FocusKeys = "all rows",
                TextContent = "পাখি ফুল খাচ্ছে। বই পড়ছি। ভাত রান্ধা হয়েছে। মাছ খুব ভালো।",
                Difficulty = "Advanced",
                EstimatedMinutes = 12
            },
            new LessonInfo
            {
                LessonNumber = 15,
                Category = LessonCategory.BottomRow,
                Title = "সম্পূর্ণ অক্ষর - দক্ষতা",
                Description = "সব অক্ষরে দক্ষতা অর্জন করুন।",
                FocusKeys = "all rows",
                TextContent = "রাত্রি শেষ হয়েছে। সূর্য উদয় হচ্ছে। পাখিরা গান গাইছে। নতুন দিন শুরু।",
                Difficulty = "Advanced",
                EstimatedMinutes = 15
            },

            // NUMBERS (Lessons 16-18)
            new LessonInfo
            {
                LessonNumber = 16,
                Category = LessonCategory.Numbers,
                Title = "সংখ্যা - মৌলিক",
                Description = "০ থেকে ৯ পর্যন্ত বাংলা সংখ্যা লিখুন।",
                FocusKeys = "1 2 3 4 5 6 7 8 9 0",
                TextContent = "০ ১ ২ ৩ ৪ ৫ ৬ ৭ ৮ ৯ ০১২৩৪৫৬৭৮৯",
                Difficulty = "Intermediate",
                EstimatedMinutes = 8
            },
            new LessonInfo
            {
                LessonNumber = 17,
                Category = LessonCategory.Numbers,
                Title = "সংখ্যা - বড় সংখ্যা",
                Description = "বড় সংখ্যা লিখতে শিখুন।",
                FocusKeys = "numbers",
                TextContent = "১০ ২০ ৫০ ১০০ ২০০ ৫০০ ১০০০ ২০২৬ ১৯৭১ ৩৬৫",
                Difficulty = "Intermediate",
                EstimatedMinutes = 10
            },
            new LessonInfo
            {
                LessonNumber = 18,
                Category = LessonCategory.Numbers,
                Title = "সংখ্যা + অক্ষর মিশ্রণ",
                Description = "সংখ্যা এবং অক্ষর একসাথে ব্যবহার করুন।",
                FocusKeys = "numbers + letters",
                TextContent = "আমার ১২ বছর বয়স। ঢাকায় ১৫০ স্কুল আছে। আজ ২০২৬ সাল।",
                Difficulty = "Advanced",
                EstimatedMinutes = 12
            },

            // PUNCTUATION (Lessons 19-20)
            new LessonInfo
            {
                LessonNumber = 19,
                Category = LessonCategory.Punctuation,
                Title = "যতিচিহ্ন - মৌলিক",
                Description = "দাঁড়ি, কমা, প্রশ্নবোধক চিহ্ন শিখুন।",
                FocusKeys = ". , ? ! ;",
                TextContent = "হ্যাঁ। না। কেমন আছেন? খুব ভালো! আসুন, বসুন।",
                Difficulty = "Intermediate",
                EstimatedMinutes = 8
            },
            new LessonInfo
            {
                LessonNumber = 20,
                Category = LessonCategory.Punctuation,
                Title = "যতিচিহ্ন - উন্নত",
                Description = "উদ্ধৃতি, কোলন, হাইফেন ইত্যাদি শিখুন।",
                FocusKeys = "\" ' : - ( )",
                TextContent = "তিনি বললেন, \"এখনই আসছি।\" সময়: ১০টা। (খুব জরুরী)",
                Difficulty = "Advanced",
                EstimatedMinutes = 10
            },

            // JUKTAKKHOR (Lessons 21-24)
            new LessonInfo
            {
                LessonNumber = 21,
                Category = LessonCategory.Juktakkhor,
                Title = "যুক্তাক্ষর - সহজ",
                Description = "সাধারণ যুক্তাক্ষর (ক্ক, ক্ত, ত্ত, ন্ত) শিখুন।",
                FocusKeys = "conjuncts",
                TextContent = "পক্ক রক্ত শক্তি সত্য অন্ত বন্ধ পন্থা ছন্দ",
                Difficulty = "Advanced",
                EstimatedMinutes = 12
            },
            new LessonInfo
            {
                LessonNumber = 22,
                Category = LessonCategory.Juktakkhor,
                Title = "যুক্তাক্ষর - মাঝারি",
                Description = "মাঝারি কঠিন যুক্তাক্ষর (স্ক, স্ট, ঞ্জ, ঙ্গ) প্র্যাকটিস করুন।",
                FocusKeys = "conjuncts",
                TextContent = "স্কুল স্টেশন অঞ্জন সঙ্গে লঙ্কা বঙ্গ",
                Difficulty = "Advanced",
                EstimatedMinutes = 15
            },
            new LessonInfo
            {
                LessonNumber = 23,
                Category = LessonCategory.Juktakkhor,
                Title = "যুক্তাক্ষর - কঠিন",
                Description = "কঠিন যুক্তাক্ষর (ক্ষ, জ্ঞ, ঞ্চ, ত্র) শিখুন।",
                FocusKeys = "conjuncts",
                TextContent = "ক্ষমা জ্ঞান পঞ্চম ত্রাণ ব্যক্তি বিশ্ব",
                Difficulty = "Expert",
                EstimatedMinutes = 18
            },
            new LessonInfo
            {
                LessonNumber = 24,
                Category = LessonCategory.Juktakkhor,
                Title = "যুক্তাক্ষর - বিশেষজ্ঞ",
                Description = "খুব জটিল যুক্তাক্ষর (ক্ষ্ম, ত্ত্ব, ন্ত্র) আয়ত্ত করুন।",
                FocusKeys = "complex conjuncts",
                TextContent = "লক্ষ্মী সত্ত্বা মন্ত্র স্বতন্ত্র কার্ত্তিক",
                Difficulty = "Expert",
                EstimatedMinutes = 20
            },

            // COMMON WORDS (Lessons 25-26)
            new LessonInfo
            {
                LessonNumber = 25,
                Category = LessonCategory.CommonWords,
                Title = "সাধারণ শব্দ - দৈনন্দিন",
                Description = "প্রতিদিন ব্যবহৃত সাধারণ শব্দ প্র্যাকটিস করুন।",
                FocusKeys = "all keys",
                TextContent = "মানুষ ঘর বাড়ি খাবার পানি কাজ সময় আজ কাল আমি তুমি সে আমরা",
                Difficulty = "Advanced",
                EstimatedMinutes = 15
            },
            new LessonInfo
            {
                LessonNumber = 26,
                Category = LessonCategory.CommonWords,
                Title = "সাধারণ শব্দ - ক্রিয়া",
                Description = "সাধারণ ক্রিয়া পদ লিখতে শিখুন।",
                FocusKeys = "all keys",
                TextContent = "করা যাওয়া আসা খাওয়া পড়া লেখা দেখা শোনা বলা চলা থাকা হওয়া",
                Difficulty = "Advanced",
                EstimatedMinutes = 15
            },

            // PHRASES (Lessons 27-28)
            new LessonInfo
            {
                LessonNumber = 27,
                Category = LessonCategory.Phrases,
                Title = "বাক্যাংশ - সাধারণ",
                Description = "দৈনন্দিন ব্যবহৃত বাক্যাংশ প্র্যাকটিস করুন।",
                FocusKeys = "all keys",
                TextContent = "কেমন আছেন? ভালো আছি। আপনার নাম কি? আমার নাম রহিম। কোথায় যাচ্ছেন?",
                Difficulty = "Advanced",
                EstimatedMinutes = 18
            },
            new LessonInfo
            {
                LessonNumber = 28,
                Category = LessonCategory.Phrases,
                Title = "বাক্যাংশ - দীর্ঘ",
                Description = "দীর্ঘ বাক্যাংশ এবং জটিল বাক্য লিখুন।",
                FocusKeys = "all keys",
                TextContent = "আমি প্রতিদিন সকালে হাঁটতে যাই। বাংলাদেশ একটি সুন্দর দেশ। আমরা মুক্তিযুদ্ধ করে স্বাধীন হয়েছি।",
                Difficulty = "Expert",
                EstimatedMinutes = 20
            },

            // FULL PARAGRAPHS (Lessons 29-30)
            new LessonInfo
            {
                LessonNumber = 29,
                Category = LessonCategory.Paragraphs,
                Title = "অনুচ্ছেদ - ছোট",
                Description = "ছোট অনুচ্ছেদ টাইপ করুন।",
                FocusKeys = "all keys",
                TextContent = "বাংলাদেশ দক্ষিণ এশিয়ার একটি দেশ। এর রাজধানী ঢাকা। বাংলা আমাদের মাতৃভাষা। ১৯৭১ সালে আমরা স্বাধীনতা অর্জন করি। আমাদের জাতীয় ফুল শাপলা।",
                Difficulty = "Expert",
                EstimatedMinutes = 25
            },
            new LessonInfo
            {
                LessonNumber = 30,
                Category = LessonCategory.Paragraphs,
                Title = "অনুচ্ছেদ - দীর্ঘ মাস্টার",
                Description = "চূড়ান্ত চ্যালেঞ্জ - দীর্ঘ অনুচ্ছেদ দ্রুত এবং নির্ভুলভাবে টাইপ করুন।",
                FocusKeys = "all keys",
                TextContent = "একুশে ফেব্রুয়ারি আমাদের জাতীয় শোক দিবস। ১৯৫২ সালের এই দিনে ভাষা শহীদরা মাতৃভাষা রক্ষার জন্য জীবন দিয়েছিলেন। তাদের আত্মত্যাগের ফলে বাংলা ভাষা আজ রাষ্ট্রভাষার মর্যাদা পেয়েছে। ২১শে ফেব্রুয়ারি এখন আন্তর্জাতিক মাতৃভাষা দিবস হিসেবে পালিত হয়। সারা বিশ্ব এই দিনে ভাষা শহীদদের স্মরণ করে। আমরা গর্বের সাথে বলতে পারি যে আমরা বাংলা ভাষায় কথা বলি।",
                Difficulty = "Master",
                EstimatedMinutes = 30
            }
        };
    }
}
