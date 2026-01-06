using UnityEditor;

namespace Southbyte.Editor
{
    public static class BuildDefinesSetter
    {
        private const string ProdBuildDefine = "PROD";
        private const string ProdEnablingMenuPath = EditorMenuNames.BuildPipelineUtilsRoot + "PROD";
        private const string ProdDisablingMenuPath = EditorMenuNames.BuildPipelineUtilsRoot + "NOT PROD";
        
        
        [MenuItem(ProdDisablingMenuPath, true)]
        public static bool CheckIsProdEnabled()
        {
            return BuildDefinitionsUtility.IsDefined(ProdBuildDefine);
        }
        
        [MenuItem(ProdEnablingMenuPath, true)]
        public static bool CheckIsProdDisabled()
        {
            return !BuildDefinitionsUtility.IsDefined(ProdBuildDefine);
        }
        
        [MenuItem(ProdEnablingMenuPath, priority = 0)]
        public static void SetBuildAsProd()
        {
            BuildDefinitionsUtility.EditBuildDefinitions(defines => defines.AddIfNotPresent(ProdBuildDefine));
        }
        
        [MenuItem(ProdDisablingMenuPath, priority = 1)]
        public static void SetBuildAsNotProd()
        {
            BuildDefinitionsUtility.EditBuildDefinitions(defines => defines.Remove(ProdBuildDefine));
        }
    }
}