﻿/*
The MIT License (MIT)

Copyright (c) 2017 Roaring Fangs LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using Anima2D;
using UnityEngine;

namespace RoaringFangs.Animation
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteMeshInstance))]
    public class SpriteMeshSelector :
        MonoBehaviour,
        ISerializationCallbackReceiver
    {
        [SerializeField]
        private SpriteMeshInstance _SpriteMeshInstance;

        private SpriteMesh _PreviousSpriteMesh;

        [SerializeField]
        private GameObject _BindingObject;

        public GameObject BindingObject
        {
            get { return _BindingObject; }
            set { _BindingObject = value; }
        }

        private static SpriteMeshBinding GetBinding(GameObject binding_object)
        {
            if (binding_object == null)
                return null;
            return binding_object.GetComponent<SpriteMeshBinding>();
        }

        public void Start()
        {
            // Initial state
            var binding = GetBinding(BindingObject);
            if (binding != null)
                _PreviousSpriteMesh = binding.Mesh;
        }

        public void Update()
        {
            var binding = GetBinding(BindingObject);
            if (binding == null)
                return;
            var mesh = binding.Mesh;
            if (mesh != _PreviousSpriteMesh)
            {
                _SpriteMeshInstance.spriteMesh = mesh;
                _PreviousSpriteMesh = mesh;
            }
        }

        public void OnBeforeSerialize()
        {
            // Script execution order interferes with GetComponent
            // Fallback to existing value if GetComponent returned null
            _SpriteMeshInstance =
                GetComponent<SpriteMeshInstance>() ??
                _SpriteMeshInstance;
        }

        public void OnAfterDeserialize()
        {
        }
    }
}