// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System;

namespace NPlug.Interop;

internal static unsafe partial class LibVst
{
    public partial struct IString
    {
        private static partial void setText8_ccw(IString* self, byte* text)
        {
            throw new NotImplementedException();
        }
        
        private static partial void setText16_ccw(IString* self, char* text)
        {
            throw new NotImplementedException();
        }
        
        private static partial byte* getText8_ccw(IString* self)
        {
            throw new NotImplementedException();
        }
        
        private static partial char* getText16_ccw(IString* self)
        {
            throw new NotImplementedException();
        }
        
        private static partial void take_ccw(IString* self, void* s, byte isWide)
        {
            throw new NotImplementedException();
        }
        
        private static partial byte isWideString_ccw(IString* self)
        {
            throw new NotImplementedException();
        }
    }
}