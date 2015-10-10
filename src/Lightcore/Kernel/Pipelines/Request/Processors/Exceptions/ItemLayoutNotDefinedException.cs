using System;
using Lightcore.Kernel.Data;

namespace Lightcore.Kernel.Pipelines.Request.Processors.Exceptions
{
    public class ItemLayoutNotDefinedException : Exception
    {
        public ItemLayoutNotDefinedException(Item item) : base($"Item '{item.Path}' did not have a layout defined")
        {
        }
    }
}