﻿
namespace GibbsPhenomenonTest.Core
{
    namespace Delegate
    {
        public delegate B Function1Continuous<B, A1>(A1 A);
        public delegate B Function1Discrete<B, A1, N1>(A1 A, N1 N);
    }
}
