using ModuleB;
using ModuleC;

namespace ModuleA
{
    public class A
    {
        public void MethodA()
        {
            var b = new B();
            b.MethodB();

            var c = new C();
            c.MethodC();
            c.MethodPajas();
        }
    }
}
