﻿using System.Text;
using System.Text.Json;

namespace TranslatorBot
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            var sb = new StringBuilder();
            var state = SeparatedCaseState.Start;
            for (var i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    if (state != SeparatedCaseState.Start)
                    {
                        state = SeparatedCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(name[i]))
                {
                    switch (state)
                    {
                        case SeparatedCaseState.Upper:
                            bool hasNext = (i + 1 < name.Length);
                            if (i > 0 && hasNext)
                            {
                                char nextChar = name[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != '_')
                                {
                                    sb.Append('_');
                                }
                            }
                            break;
                        case SeparatedCaseState.Lower:
                        case SeparatedCaseState.NewWord:
                            sb.Append('_');
                            break;
                    }
                    var c = char.ToLowerInvariant(name[i]);
                    sb.Append(c);
                    state = SeparatedCaseState.Upper;
                }
                else if (name[i] == '_')
                {
                    sb.Append('_');
                    state = SeparatedCaseState.Start;
                }
                else
                {
                    if (state == SeparatedCaseState.NewWord)
                    {
                        sb.Append('_');
                    }

                    sb.Append(name[i]);
                    state = SeparatedCaseState.Lower;
                }
            }
            return sb.ToString();
        }

        private enum SeparatedCaseState
        {
            Start,
            Lower,
            Upper,
            NewWord
        }
    }
}