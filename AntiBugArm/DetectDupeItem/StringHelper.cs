namespace DetectDupeItem
{
    internal static class StringHelper
    {
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            string[] arr1 = new string[] {
                "aì", "aÌ", "aÒ", "aÞ", "aò", "â", "âì", "âÌ", "âÒ", "âÞ", "âò", "ã", "ãì", "ãÌ", "ãÒ", "ãÞ", "ãò", "aÌ", "aÞ",
                "ð",
                "eì","eÌ","eÒ","eÞ","eò","ê","êì","êÌ","êÒ","êÞ","êò",
                "iì","iÌ","iÒ","iÞ","iò",
                "oì","oÌ","oÒ","oÞ","oò","ô","ôì","ôÌ","ôÒ","ôÞ","ôò","õ","õì","õÌ","õÒ","õÞ","õò", "oì", "oÞ", "oÌ", "oò", "oÒ",
                "uì","uÌ","uÒ","uÞ","uò","ý","ýì","ýÌ","ýÒ","ýÞ","ýò", "yì", "uÌ", "uÞ", "uÒ", "uþ", "uÞ", "uÒ", "uÞ",
                "ý","ỳ","ỷ","ỹ","ỵ", "yÌ",
                "uò",
                "eì", "eò", "eò", "eÒ", "eÌ",
                "uì",
                "aì","aò",
                "oÞ"
            };
            string[] arr2 = new string[] {
                "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o", "o", "o", "o", "o", "o",
                "u","u","u","u","u","u","u","u","u","u","u", "u", "u", "u", "u", "u", "u", "u", "u",
                "y","y","y","y","y", "y",
                "u",
                "e", "e", "e", "e", "e",
                "u",
                "a", "a",
                "o"
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return NonUnicodeUtf8(text);
        }

        public static string NonUnicodeUtf8(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ"
            };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y"
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return NonUnicode(text);
        }
        public static string NonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ"
            };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y"
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
    }
}
