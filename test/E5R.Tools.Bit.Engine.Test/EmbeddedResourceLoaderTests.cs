using System;
using System.Reflection;
using Xunit;

namespace E5R.Tools.Bit.Engine.Test
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tools.Bit.Engine")]
    [Trait(_.COMPONENT, nameof(EmbeddedResourceLoader))]
    [Trait(_.CATEGORY, "UNIT")]
    public class EmbeddedResourceLoaderTests
    {
        [Fact(DisplayName = "Constructor is private")]
        public void Constructor_Is_Private()
        {
            var type = typeof(EmbeddedResourceLoader);
            var paramTypes = new Type[] { typeof(Assembly), typeof(string) };

            Assert.Null(type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, paramTypes, null));
            Assert.NotNull(type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, paramTypes, null));
        }

        [Fact(DisplayName = "EmbeddedFrom require obj parameter")]
        public void EmbeddedFrom_Require_type()
        {
            var error = Assert.Throws<ArgumentNullException>(() => EmbeddedResourceLoader.EmbeddedFrom(null));
            Assert.Equal("obj", error.ParamName);
            Assert.NotNull(EmbeddedResourceLoader.EmbeddedFrom(this));
        }
    }
}
