using System.Collections.Generic;
using System.Reflection;
using Android.Content.Res;
using Android.Util;
using Java.Lang;
using StringBuilder = System.Text.StringBuilder;

namespace Mono.Android.Crasher.Utils
{
    class ConfigurationInspector
    {
        private const string SUFFIX_MASK = "_MASK";
        private const string FIELD_SCREENLAYOUT = "screenLayout";
        private const string FIELD_UIMODE = "uiMode";
        private const string FIELD_MNC = "mnc";
        private const string FIELD_MCC = "mcc";
        private const string PREFIX_UI_MODE = "UI_MODE_";
        private const string PREFIX_TOUCHSCREEN = "TOUCHSCREEN_";
        private const string PREFIX_SCREENLAYOUT = "SCREENLAYOUT_";
        private const string PREFIX_ORIENTATION = "ORIENTATION_";
        private const string PREFIX_NAVIGATIONHIDDEN = "NAVIGATIONHIDDEN_";
        private const string PREFIX_NAVIGATION = "NAVIGATION_";
        private const string PREFIX_KEYBOARDHIDDEN = "KEYBOARDHIDDEN_";
        private const string PREFIX_KEYBOARD = "KEYBOARD_";
        private const string PREFIX_HARDKEYBOARDHIDDEN = "HARDKEYBOARDHIDDEN_";
        private static readonly IDictionary<int, string> _mHardKeyboardHiddenValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mKeyboardValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mKeyboardHiddenValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mNavigationValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mNavigationHiddenValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mOrientationValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mScreenLayoutValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mTouchScreenValues = new Dictionary<int, string>();
        private static readonly IDictionary<int, string> _mUiModeValues = new Dictionary<int, string>();

        private static readonly Dictionary<string, IDictionary<int, string>> _mValueArrays = new Dictionary<string, IDictionary<int, string>>();

        static ConfigurationInspector()
        {
            _mValueArrays.Add(PREFIX_HARDKEYBOARDHIDDEN, _mHardKeyboardHiddenValues);
            _mValueArrays.Add(PREFIX_KEYBOARD, _mKeyboardValues);
            _mValueArrays.Add(PREFIX_KEYBOARDHIDDEN, _mKeyboardHiddenValues);
            _mValueArrays.Add(PREFIX_NAVIGATION, _mNavigationValues);
            _mValueArrays.Add(PREFIX_NAVIGATIONHIDDEN, _mNavigationHiddenValues);
            _mValueArrays.Add(PREFIX_ORIENTATION, _mOrientationValues);
            _mValueArrays.Add(PREFIX_SCREENLAYOUT, _mScreenLayoutValues);
            _mValueArrays.Add(PREFIX_TOUCHSCREEN, _mTouchScreenValues);
            _mValueArrays.Add(PREFIX_UI_MODE, _mUiModeValues);

            foreach (var f in typeof(Configuration).GetFields())
            {
                if (f.IsStatic)
                {
                    var fieldName = f.Name;
                    try
                    {
                        if (fieldName.StartsWith(PREFIX_HARDKEYBOARDHIDDEN))
                        {
                            _mHardKeyboardHiddenValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_KEYBOARD))
                        {
                            _mKeyboardValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_KEYBOARDHIDDEN))
                        {
                            _mKeyboardHiddenValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_NAVIGATION))
                        {
                            _mNavigationValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_NAVIGATIONHIDDEN))
                        {
                            _mNavigationHiddenValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_ORIENTATION))
                        {
                            _mOrientationValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_SCREENLAYOUT))
                        {
                            _mScreenLayoutValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_TOUCHSCREEN))
                        {
                            _mTouchScreenValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                        else if (fieldName.StartsWith(PREFIX_UI_MODE))
                        {
                            _mUiModeValues.Add(int.Parse(f.GetValue(null).ToString()), fieldName);
                        }
                    }
                    catch (IllegalArgumentException e)
                    {
                        Log.Warn(Constants.LOG_TAG, "Error while inspecting device configuration: ", e);
                    }
                    catch (IllegalAccessException e)
                    {
                        Log.Warn(Constants.LOG_TAG, "Error while inspecting device configuration: ", e);
                    }
                }
            }
        }

        public static string ToString(Configuration conf)
        {
            var result = new StringBuilder();
            foreach (var f in conf.GetType().GetFields())
            {
                try
                {
                    if (!f.IsStatic)
                    {
                        var fieldName = f.Name;
                        result.Append(fieldName).Append('=');
                        if (f.GetType().Equals(typeof(int)))
                        {
                            result.Append(GetFieldValueName(conf, f));
                        }
                        else
                        {
                            result.Append(f.GetValue(conf).ToString());
                        }
                        result.Append('\n');
                    }
                }
                catch (IllegalArgumentException e)
                {
                    Log.Error(Constants.LOG_TAG, "Error while inspecting device configuration: ", e);
                }
                catch (IllegalAccessException e)
                {
                    Log.Error(Constants.LOG_TAG, "Error while inspecting device configuration: ", e);
                }
            }
            return result.ToString();
        }

        private static string GetFieldValueName(Configuration conf, FieldInfo f)
        {
            var fieldName = f.Name;
            if (fieldName.Equals(FIELD_MCC) || fieldName.Equals(FIELD_MNC))
            {
                return f.GetValue(conf).ToString();
            }
            if (fieldName.Equals(FIELD_UIMODE))
            {
                return ActiveFlags(_mValueArrays[PREFIX_UI_MODE], int.Parse(f.GetValue(conf).ToString()));
            }
            if (fieldName.Equals(FIELD_SCREENLAYOUT))
            {
                return ActiveFlags(_mValueArrays[PREFIX_SCREENLAYOUT], int.Parse(f.GetValue(conf).ToString()));
            }
            var values = _mValueArrays[fieldName.ToUpper() + '_'];
            if (values == null)
            {
                // Unknown field, return the raw int as String
                return f.GetValue(conf).ToString();
            }
            string value;
            values.TryGetValue(int.Parse(f.GetValue(conf).ToString()), out value);
            return value ?? f.GetValue(conf).ToString();
        }

        private static string ActiveFlags(IDictionary<int, string> valueNames, int bitfield)
        {
            var result = new StringBuilder();
            foreach (var key in valueNames.Keys)
            {
                if (valueNames[key].EndsWith(SUFFIX_MASK))
                {
                    var value = bitfield & key;
                    if (value > 0)
                    {
                        if (result.Length > 0)
                        {
                            result.Append('+');
                        }
                        result.Append(valueNames[value]);
                    }
                }
            }
            return result.ToString();
        }
    }
}