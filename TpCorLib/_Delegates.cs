using System;
using System.Collections.Generic;
using System.Text;

namespace Teleperformance
{
    public delegate void StateAction<T>(T state);

    public delegate R StateActionWithReturn<T, R>(T state);
}
