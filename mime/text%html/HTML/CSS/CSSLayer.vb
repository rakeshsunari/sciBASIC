﻿#Region "Microsoft.VisualBasic::b4bd9e31aeda91e6f57bb85044c80423, mime\text%html\HTML\CSS\CSSLayer.vb"

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

    '     Interface CSSLayer
    ' 
    '         Properties: zIndex
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Xml.Serialization

Namespace HTML.CSS

    ''' <summary>
    ''' 进行样式渲染的图层对象
    ''' </summary>
    Public Interface CSSLayer

        ''' <summary>
        ''' Drawing order, if this index value is greater, then it will be draw on the top most.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("z-index")>
        Property zIndex As Integer

    End Interface
End Namespace
