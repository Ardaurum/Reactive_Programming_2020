using System;
using System.Collections.Generic;
using _01_WaitForMouse;
using _02_WaitForTask;
using _03_MouseDrag;

namespace Runner
{
    public enum ExampleType
    {
        MouseCallback,
        MouseReactive,
        MouseDragCallback,
        MouseDragReactive,
        WaitForAsyncAwait,
        WaitForCallback,
        WaitForCoroutine,
        WaitForTask,
        WaitForReactive
    }
    
    public static class ExampleRepository
    {
        private static readonly Dictionary<ExampleType, Type> Examples = new Dictionary<ExampleType, Type>
        {
            { ExampleType.MouseCallback, typeof(MouseCallback) },
            { ExampleType.MouseReactive, typeof(MouseReactive) },
            { ExampleType.MouseDragCallback, typeof(MouseDragCallbackHandler) },
            { ExampleType.MouseDragReactive, typeof(MouseDragReactiveHandler) },
            { ExampleType.WaitForAsyncAwait, typeof(WaitForAsyncAwait) },
            { ExampleType.WaitForCallback, typeof(WaitForCallback) },
            { ExampleType.WaitForCoroutine, typeof(WaitForCoroutine) },
            { ExampleType.WaitForTask, typeof(WaitForTask) },
            { ExampleType.WaitForReactive, typeof(WaitForReactive) }
        };

        public static object GetExample(ExampleType type)
        {
            return Activator.CreateInstance(Examples[type]);
        }
    }
}