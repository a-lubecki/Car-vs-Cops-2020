using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using UnityEngine.Events;

namespace TigerForge
{
    public class EasyFileSave
    {

        #region " VARIABLES & PROPERTIES "

        private Dictionary<string, object> storage = new Dictionary<string, object>();

        /// <summary>
        /// The error information.
        /// </summary>
        public string Error = "";

        /// <summary>
        /// Disable the warning messages shown in the Console. 
        /// </summary>
        public bool suppressWarning = true;

        private readonly string fileName = "";

        public struct UnityTransform
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 localScale;
            public Vector3 localPosition;
            public Quaternion localRotation;
            public Vector3 lossyScale;
            public Vector3 eulerAngles;
            public Vector3 localEulerAngles;
        }

        private EasyFileSaveExtension customs = new EasyFileSaveExtension();

        public struct CustomData
        {
            /// <summary>
            /// The raw object data.
            /// </summary>
            public object data;

            /// <summary>
            /// Cast the object into integer value.
            /// </summary>
            /// <returns></returns>
            public int ToInt()
            {
                try
                {
                    return (int)data;
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }

            /// <summary>
            /// Cast the object into float value.
            /// </summary>
            /// <returns></returns>
            public float ToFloat()
            {
                try
                {
                    return (float)data;
                }
                catch (System.Exception)
                {
                    return 0f;
                }
            }

            /// <summary>
            /// Cast the object into byrte value.
            /// </summary>
            /// <returns></returns>
            public byte ToByte()
            {
                try
                {
                    return (byte)data;
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }

            /// <summary>
            /// Cast the object into a string.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                try
                {
                    return (string)data;
                }
                catch (System.Exception)
                {
                    return "";
                }
            }

            /// <summary>
            /// Cast the object into a boolean value.
            /// </summary>
            /// <returns></returns>
            public bool ToBool()
            {
                try
                {
                    return (bool)data;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }

        }

        #endregion


        #region " CONSTRUCTOR "

        /// <summary>
        /// Initialize a new instance with the given fileName or with a default file name if it's not specified.
        /// </summary>
        /// <param name="fileName"></param>
        public EasyFileSave(string fileName = "")
        {
            if (fileName == "") fileName = "gamedata";

            storage = new Dictionary<string, object>();
            this.fileName = Application.persistentDataPath + "/" + fileName + ".dat";
            Error = "";
            Register(fileName);

            customs.Start();
        }

        #endregion
        

        #region " SAVE "

        /// <summary>
        /// Save the 'system internal storage' data into the file. Return TRUE when done without errors.
        /// </summary>
        public bool Save()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream saveFile = File.Create(fileName);
                bf.Serialize(saveFile, storage);
                saveFile.Close();
                Dispose();
                return true;
            }
            catch (System.Exception e)
            {
                Error = "[Easy File Save] This system exeption has been thrown during saving: " + e.Message;
                return false;
            }
            
        }

        /// <summary>
        /// Append the 'system internal storage' data at the end of the current file content. By default, existing keys will be overwritten with new values. If 'overwrite' parameter is set to FALSE, existing keys will be ignored.
        /// </summary>
        public bool Append(bool overwrite = true)
        {
            try
            {
                Dictionary<string, object> fileStorage = new Dictionary<string, object>();

                if (FileExists())
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream openFile = File.Open(fileName, FileMode.Open);
                    fileStorage = (Dictionary<string, object>)bf2.Deserialize(openFile);
                    openFile.Close();

                    foreach (KeyValuePair<string, object> item in storage)
                    {
                        if (fileStorage.ContainsKey(item.Key))
                        {
                            if (overwrite) fileStorage[item.Key] = item.Value;
                        }
                        else
                        {
                            fileStorage.Add(item.Key, item.Value);
                        }
                    }
                }
                else
                {
                    fileStorage = storage;
                }

                BinaryFormatter bf = new BinaryFormatter();
                FileStream saveFile = File.Create(fileName);
                bf.Serialize(saveFile, fileStorage);
                saveFile.Close();
                Dispose();
                return true;
            }
            catch (System.Exception e)
            {
                Error = "[Easy File Save] This system exeption has been thrown during append data: " + e.Message;
                return false;
            }

            
        }

        /// <summary>
        /// Add a new value, with the given unique key, into the 'system internal storage'.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            if (KeyExists(key))
            {
                Debug.LogWarning("[Easy File Save] Trying to reuse the key '" + key + "' to put a value in the storage!");
            }
            else
            {
                value = ConvertUnityTypes(value);
                storage.Add(key, value);
            }
        }

        

        #endregion


        #region " LOAD "

        /// <summary>
        /// Load the file data into the 'system internal storage' and return TRUE if the loading has been completed. Return FALSE if something has gone wrong (use Error property to get error informations).
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (!FileExists())
            {
                Error = "[Easy File Save] The file " + fileName + " doesn't exist.";
                return false;
            }

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream saveFile = File.Open(fileName, FileMode.Open);

                storage = (Dictionary<string, object>)bf.Deserialize(saveFile);

                saveFile.Close();
                return true;
            }
            catch (System.Exception e)
            {
                Error = "[Easy File Save] This system exeption has been thrown during loading: " + e.Message;
                return false;
            }

        }

        #endregion


        #region " TOOLS "

        /// <summary>
        /// Return TRUE if the file exists.
        /// </summary>
        /// <returns></returns>
        public bool FileExists()
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Delete this file (if it exists).
        /// </summary>
        public void Delete()
        {
            if (FileExists())
            {
                File.Delete(fileName);
                Dispose();
            }
        }

        /// <summary>
        /// Return TRUE if the 'system internal storage' contains the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return storage.ContainsKey(key);
        }

        /// <summary>
        /// Return the system internal storage.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetStorage()
        {
            return storage;
        }

        /// <summary>
        /// Return the current file name with path.
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return fileName;
        }

        /// <summary>
        /// Delete the 'system internal storage' so to free this part of memory. This method is automatically called after saving. It should be manually called after loading data.
        /// </summary>
        public void Dispose()
        {
            storage = new Dictionary<string, object>();
            Error = "";
        }

        private void Warning(string message)
        {
            if (!suppressWarning) Debug.LogWarning(message);
        }

        #endregion


        #region " GETTERS (DEFAULT TYPES) "

        /// <summary>
        /// Return the object data for the given key (or null if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            try
            {
                if (storage.ContainsKey(key)) return storage[key]; else return null;
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetData error using key: " + key);
                return null;
            }
        }

        /// <summary>
        /// Return the integer data for the given key (or 0 if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            try
            {
                if (storage.ContainsKey(key)) return (int)storage[key]; else return 0;
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetInt error using key: " + key);
                return 0;
            }
        }

        /// <summary>
        /// Return the boolean data for the given key (or false if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetBool(string key, bool defaultValue = false)
        {
            try
            {
                if (storage.ContainsKey(key)) return (bool)storage[key]; else return defaultValue;
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetBool error using key: " + key);
                return defaultValue;
            }
        }

        /// <summary>
        /// Return the float data for the given key (or 0 if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetFloat(string key)
        {
            try
            {
                if (storage.ContainsKey(key)) return (float)storage[key]; else return 0f;
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetFloat error using key: " + key);
                return 0f;
            }
        }

        /// <summary>
        /// Return the string data for the given key (or "" if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            try
            {
                if (storage.ContainsKey(key)) return (string)storage[key]; else return "";
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetString error using key: " + key);
                return "";
            }
        }

        /// <summary>
        /// Return the byte data for the given key (or 0 if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte GetByte(string key)
        {
            try
            {
                if (storage.ContainsKey(key)) return (byte)storage[key]; else return 0;
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetByte error using key: " + key);
                return 0;
            }
        }

        #endregion


        #region " SPECIAL TYPES (SETTERS & GETTERS) "

        /// <summary>
        /// Serialize an object and add it, with the given unique key, into the 'system internal storage'.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void AddSerialized(string key, object data)
        {
            var xml = Serialize(data);
            Add(key, xml);
        }

        /// <summary>
        /// Return the object data, for the given key, deserialized with the given type (or null if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetDeserialized(string key, System.Type type)
        {
            try
            {
                var obj = GetData(key);
                if (obj != null) return Deserialize(obj, type); else return null;
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetDeserializedObject error using key: " + key);
                return null;
            }
        }

        /// <summary>
        /// Add a new custom value, with the given unique key, into the 'system internal storage'.
        /// </summary>
        public void AddCustom(string key, object data, string extensionName)
        {
            if (!customs.extensions.ContainsKey(extensionName))
            {
                Debug.LogWarning("[Easy File Save] AddCustom: an extension with name '" + extensionName + "doesn't exist.");
                return;
            }

            UnityAction myExtension = customs.extensions[extensionName];
            customs.data[extensionName] = data;
            myExtension.Invoke();

            List<object> dataToSave = customs.pars[extensionName];
            Add(key, dataToSave);
        }

        /// <summary>
        /// Return a dictionary of custom values.
        /// </summary>
        public Dictionary<string, CustomData> GetCustom(string key, string extensionName)
        {
            try
            {
                if (storage.ContainsKey(key)) {

                    if (!customs.mapping.ContainsKey(extensionName))
                    {
                        Debug.LogWarning("[Easy File Save] GetCustom: an extension with name '" + extensionName + "doesn't exist.");
                        return null;
                    }

                    List<object> dataToLoad = (List<object>)storage[key];
                    List<string> mapping = customs.mapping[extensionName];

                    if (dataToLoad.Count != mapping.Count)
                    {
                        Debug.LogWarning("[Easy File Save] GetCustom: check your extension! Something gone wrong.");
                        return null;
                    }

                    Dictionary<string, CustomData> customData = new Dictionary<string, CustomData>();
                    for (var i = 0; i < mapping.Count; i++)
                    {
                        customData.Add(mapping[i], new CustomData { data = dataToLoad[i] });
                    }

                    return customData;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetCustom error using key: " + key);
            }
            return null;
        }

        #endregion


        #region  " UNITY TYPES (SETTERS & GETTERS) "

        private object ConvertUnityTypes(object value)
        {
            string type = value.GetType().ToString();
            if (!type.StartsWith("UnityEngine")) return value;

            List<float> converted = new List<float>();

            switch (type)
            {
                case "UnityEngine.Vector2":
                    Vector2 v2Data = (Vector2)value;
                    converted.Add(v2Data.x);
                    converted.Add(v2Data.y);

                    break;

                case "UnityEngine.Vector3":
                    Vector3 v3Data = (Vector3)value;
                    converted.Add(v3Data.x);
                    converted.Add(v3Data.y);
                    converted.Add(v3Data.z);

                    break;

                case "UnityEngine.Vector4":
                    Vector4 v4Data = (Vector4)value;
                    converted.Add(v4Data.x);
                    converted.Add(v4Data.y);
                    converted.Add(v4Data.z);
                    converted.Add(v4Data.w);

                    break;

                case "UnityEngine.Quaternion":
                    Quaternion qData = (Quaternion)value;
                    converted.Add(qData.x);
                    converted.Add(qData.y);
                    converted.Add(qData.z);
                    converted.Add(qData.w);

                    break;

                case "UnityEngine.Transform":
                    Transform trData = (Transform)value;
                    converted.Add(trData.position.x);
                    converted.Add(trData.position.y);
                    converted.Add(trData.position.z);
                    converted.Add(trData.localPosition.x);
                    converted.Add(trData.localPosition.y);
                    converted.Add(trData.localPosition.z);
                    converted.Add(trData.localScale.x);
                    converted.Add(trData.localScale.y);
                    converted.Add(trData.localScale.z);
                    converted.Add(trData.lossyScale.x);
                    converted.Add(trData.lossyScale.y);
                    converted.Add(trData.lossyScale.z);
                    converted.Add(trData.rotation.x);
                    converted.Add(trData.rotation.y);
                    converted.Add(trData.rotation.z);
                    converted.Add(trData.rotation.w);
                    converted.Add(trData.localRotation.x);
                    converted.Add(trData.localRotation.y);
                    converted.Add(trData.localRotation.z);
                    converted.Add(trData.localRotation.w);
                    converted.Add(trData.eulerAngles.x);
                    converted.Add(trData.eulerAngles.y);
                    converted.Add(trData.eulerAngles.z);
                    converted.Add(trData.localEulerAngles.x);
                    converted.Add(trData.localEulerAngles.y);
                    converted.Add(trData.localEulerAngles.z);

                    break;

                case "UnityEngine.Color":
                    Color clData = (Color)value;
                    converted.Add(clData.r);
                    converted.Add(clData.g);
                    converted.Add(clData.b);
                    converted.Add(clData.a);

                    break;

                case "UnityEngine.Color32":
                    Color32 cl32Data = (Color32)value;
                    converted.Add(cl32Data.r);
                    converted.Add(cl32Data.g);
                    converted.Add(cl32Data.b);
                    converted.Add(cl32Data.a);

                    break;

                case "UnityEngine.Rect":
                    Rect reData = (Rect)value;
                    converted.Add(reData.x);
                    converted.Add(reData.y);
                    converted.Add(reData.width);
                    converted.Add(reData.height);

                    break;

                default:
                    break;
            }

            return converted;
        }

        /// <summary>
        /// Return the Vector2 data for the given key (or Vector2(0, 0) if nothing found).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Vector2 GetUnityVector2(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v2List = (List<float>)storage[key];
                    Vector2 v2 = new Vector2(v2List[0], v2List[1]);
                    return v2;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityVector2 error using key: " + key);
            }
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Return the Vector3 data for the given key (or Vector3(0, 0, 0) if nothing found).
        /// </summary>
        public Vector3 GetUnityVector3(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    Vector3 v3 = new Vector3(v3List[0], v3List[1], v3List[2]);
                    return v3;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityVector3 error using key: " + key);
            }
            return new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Return the Vector4 data for the given key (or Vector4(0, 0, 0, 0) if nothing found).
        /// </summary>
        public Vector4 GetUnityVector4(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    Vector4 v4 = new Vector4(v3List[0], v3List[1], v3List[2], v3List[3]);
                    return v4;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityVector4 error using key: " + key);
            }
            return new Vector4(0, 0, 0, 0);
        }

        /// <summary>
        /// Return the Quaternion data for the given key (or Quaternion(0, 0, 0, 0) if nothing found).
        /// </summary>
        public Quaternion GetUnityQuaternion(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    Quaternion q = new Quaternion(v3List[0], v3List[1], v3List[2], v3List[3]);
                    return q;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityQuaternion error using key: " + key);
            }
            return new Quaternion(0, 0, 0, 0);
        }

        /// <summary>
        /// Return the Quaternion data for the given key (or Quaternion(0, 0, 0, 0) if nothing found).
        /// </summary>
        public UnityTransform GetUnityTransform(string key)
        {
            var tr = new UnityTransform();

            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    tr.position.x = v3List[0];
                    tr.position.y = v3List[1];
                    tr.position.z = v3List[2];
                    tr.localPosition.x = v3List[3];
                    tr.localPosition.y = v3List[4];
                    tr.localPosition.z = v3List[5];
                    tr.localScale.x = v3List[6];
                    tr.localScale.y = v3List[7];
                    tr.localScale.z = v3List[8];
                    tr.lossyScale.x = v3List[9];
                    tr.lossyScale.y = v3List[10];
                    tr.lossyScale.z = v3List[11];
                    tr.rotation.x = v3List[12];
                    tr.rotation.y = v3List[13];
                    tr.rotation.z = v3List[14];
                    tr.rotation.w = v3List[15];
                    tr.localRotation.x = v3List[16];
                    tr.localRotation.y = v3List[17];
                    tr.localRotation.z = v3List[18];
                    tr.localRotation.w = v3List[19];
                    tr.eulerAngles.x = v3List[20];
                    tr.eulerAngles.y = v3List[21];
                    tr.eulerAngles.z = v3List[22];
                    tr.localEulerAngles.x = v3List[23];
                    tr.localEulerAngles.y = v3List[24];
                    tr.localEulerAngles.z = v3List[25];
                    return tr;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityTransform error using key: " + key);
            }
            return tr;
        }

        /// <summary>
        /// Return the Color data for the given key (or Color(0, 0, 0) if nothing found).
        /// </summary>
        public Color GetUnityColor(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    Color v4 = new Color(v3List[0], v3List[1], v3List[2], v3List[3]);
                    return v4;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityColor error using key: " + key);
            }
            return new Color(0, 0, 0);
        }

        /// <summary>
        /// Return the Color32 data for the given key (or Color32(0, 0, 0, 0) if nothing found).
        /// </summary>
        public Color32 GetUnityColor32(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    Color32 v4 = new Color32((byte)v3List[0], (byte)v3List[1], (byte)v3List[2], (byte)v3List[3]);
                    return v4;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityColor32 error using key: " + key);
            }
            return new Color32(0, 0, 0, 0);
        }

        /// <summary>
        /// Return the Rect data for the given key (or Rect(0, 0, 0, 0) if nothing found).
        /// </summary>
        public Rect GetUnityRect(string key)
        {
            try
            {
                if (storage.ContainsKey(key))
                {
                    List<float> v3List = (List<float>)storage[key];
                    Rect v4 = new Rect(v3List[0], v3List[1], v3List[2], v3List[3]);
                    return v4;
                }
            }
            catch (System.Exception)
            {
                Warning("[Easy File Save] GetUnityRect error using key: " + key);
            }
            return new Rect(0, 0, 0, 0);
        }

        #endregion


        #region " STATIC FUNCTIONS "

        private static List<string> filesArchive = new List<string>();

        public static void Register(string fileName)
        {
            if (!filesArchive.Contains(fileName)) filesArchive.Add(fileName);
        }

        /// <summary>
        /// Delete all the files created by Easy File Save.
        /// </summary>
        public static void DeleteAll()
        {
            foreach (string fileName in filesArchive)
            {
                if (File.Exists(fileName)) File.Delete(fileName);
            }
            filesArchive = new List<string>();
        }

        /// <summary>
        /// Serialize the object data in the proper way and return its XML structure.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Serialize(object data)
        {
            System.Type st = data.GetType();

            var sw = new StringWriter();
            XmlSerializer ser = new XmlSerializer(st);
            ser.Serialize(sw, data);
            string xml = sw.ToString();

            return xml;
        }

        /// <summary>
        /// Deserialize the given data with the given type.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(object data, System.Type type)
        {
            string xml = (string)data;

            XmlSerializer ser = new XmlSerializer(type);

            object result = null;
            using (TextReader reader = new StringReader(xml))
            {
                result = ser.Deserialize(reader);
            }

            return result;
        }

        #endregion


        #region " CONVERTERS "

        public class Converter
        {

            /// <summary>
            /// Cast the object into integer value.
            /// </summary>
            /// <returns></returns>
            public static int ToInt(object value)
            {
                try
                {
                    return (int)value;
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }

            /// <summary>
            /// Cast the object into float value.
            /// </summary>
            /// <returns></returns>
            public static float ToFloat(object value)
            {
                try
                {
                    return (float)value;
                }
                catch (System.Exception)
                {
                    return 0f;
                }
            }

            /// <summary>
            /// Cast the object into a string.
            /// </summary>
            /// <returns></returns>
            public static string ToString(object value)
            {
                try
                {
                    return (string)value;
                }
                catch (System.Exception)
                {
                    return "";
                }
            }

            /// <summary>
            /// Cast the object into a boolean value.
            /// </summary>
            /// <returns></returns>
            public static bool ToBool(object value)
            {
                try
                {
                    return (bool)value;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }

        }

        #endregion


    }
}


