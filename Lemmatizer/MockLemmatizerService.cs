using System;
using System.Collections.Generic;

namespace Lemmatizer
{
    public class MockLemmatizerService : ILemmatizerService
    {
        private readonly Dictionary<string, string> _lemmaDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // --- Йти ---
            { "йде", "йти" },
            { "йшов", "йти" },
            { "йшла", "йти" },
            { "йшли", "йти" },
            { "піду", "йти" },
            { "підеш", "йти" },

            // --- Бути ---
            { "є", "бути" },
            { "був", "бути" },
            { "була", "бути" },
            { "було", "бути" },
            { "були", "бути" },
            { "буде", "бути" },
            { "будуть", "бути" },

            // --- Людина ---
            { "люди", "людина" },
            { "людині", "людина" },
            { "людиною", "людина" },
            { "людей", "людина" },
            { "людям", "людина" },
            { "людьми", "людина" },

            // --- Світ ---
            { "світу", "світ" },
            { "світі", "світ" },
            { "світом", "світ" },
        
            // --- Дерево ---
            { "дерева", "дерево" },
            { "дереву", "дерево" },
            { "деревом", "дерево" },
            { "деревами", "дерево" },
            { "дерев", "дерево" },

            // --- Гарний ---
            { "гарна", "гарний" },
            { "гарне", "гарний" },
            { "гарні", "гарний" },
            { "гарного", "гарний" },
            { "гарним", "гарний" },
        };

        public string Lemmatize(string word)
        {
            var cleanWord = word.Trim('\'', '`', '’').ToLower();
            if (_lemmaDictionary.TryGetValue(cleanWord, out var lemma))
            {
                return lemma;
            }

            return cleanWord.ToLower();
        }
    }
}