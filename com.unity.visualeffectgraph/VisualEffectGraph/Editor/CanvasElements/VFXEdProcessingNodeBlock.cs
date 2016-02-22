using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor.Experimental;
using UnityEditor.Experimental.Graph;
using Object = UnityEngine.Object;

namespace UnityEditor.Experimental
{
    internal class VFXEdProcessingNodeBlock : VFXEdNodeBlock
    {

        public VFXBlockModel Model
        {
            get { return m_Model; }
        }

        public VFXParamValue[] ParamValues {get { return m_ParamValues; } }
        public VFXParam[] Params {get { return m_Params; } }

        private VFXBlockModel m_Model;
        private VFXParam[] m_Params;
        private VFXParamValue[] m_ParamValues;

        public VFXEdProcessingNodeBlock(VFXBlock block, VFXEdDataSource dataSource) : base(dataSource)
        {
            m_Model = new VFXBlockModel(block);
            m_ParamValues = new VFXParamValue[block.m_Params.Length];
            m_Params = block.m_Params;
            m_Fields = new VFXEdNodeBlockParameterField[block.m_Params.Length];

            // For selection
            target = ScriptableObject.CreateInstance<VFXEdProcessingNodeBlockTarget>();
            (target as VFXEdProcessingNodeBlockTarget).targetNodeBlock = this;
            
            for (int i = 0; i < m_ParamValues.Length; ++i)
            {
                m_ParamValues[i] = VFXParamValue.Create(block.m_Params[i].m_Type);
                m_Fields[i] = new VFXEdNodeBlockParameterField(dataSource as VFXEdDataSource, block.m_Params[i].m_Name, m_ParamValues[i], true, Direction.Input, i);
                AddChild(m_Fields[i]);
                m_Model.BindParam(m_ParamValues[i], i);
            }

            m_Name = block.m_Name;

            AddChild(new VFXEdNodeBlockHeader(m_Name, VFXEditor.styles.GetIcon(block.m_IconPath == "" ? "Default" : block.m_IconPath), block.m_Params.Length > 0));
            AddManipulator(new ImguiContainer());

            Layout();
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            Model.Detach();
        }

        protected override float GetHeight()
        {
            float height = VFXEditorMetrics.NodeBlockHeaderHeight;
            foreach(VFXEdNodeBlockParameterField field in m_Fields) {
                height += field.scale.y;
            }
            return height;
        }

        protected override GUIStyle GetNodeBlockStyle()
        {
            return VFXEditor.styles.NodeBlock;
        }

        protected override GUIStyle GetNodeBlockSelectedStyle()
        {
            return VFXEditor.styles.NodeBlockSelected;
        }

        public override void Render(Rect parentRect, Canvas2D canvas)
        {
            base.Render(parentRect, canvas);
        }

    }
}
