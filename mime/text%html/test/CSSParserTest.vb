﻿#Region "Microsoft.VisualBasic::2b8e50801156a99f5c8740ce9b79b0e9, mime\text%html\test\CSSParserTest.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Module CSSParserTest
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS.Parser

Module CSSParserTest

    Sub Main()

        Dim css$ = "D:\GCModeller\src\runtime\sciBASIC#\gr\SVG\demo.css"

        Dim file = CssParser.GetTagWithCSS(css.ReadAllText)


        Pause()
    End Sub
End Module
