﻿#Region "Microsoft.VisualBasic::6c99645dbbe3d3bf19078771fb9f1c88, Microsoft.VisualBasic.Core\CommandLine\Reflection\Attributes\Attribute.vb"

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

    '     Class UsageAttribute
    ' 
    '         Properties: UsageInfo
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    '     Class ExampleAttribute
    ' 
    '         Properties: ExampleInfo
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    '     Class NoteAttribute
    ' 
    '         Properties: noteText
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.ComponentModel

Namespace CommandLine.Reflection

    <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)>
    Public Class UsageAttribute : Inherits Attribute

        Public ReadOnly Property UsageInfo As String

        Sub New(usage$)
            UsageInfo = usage
        End Sub

        Public Overrides Function ToString() As String
            Return UsageInfo
        End Function
    End Class

    <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)>
    Public Class ExampleAttribute : Inherits Attribute

        Public ReadOnly Property ExampleInfo As String

        Sub New(note$)
            ExampleInfo = note
        End Sub

        Public Overrides Function ToString() As String
            Return ExampleInfo
        End Function
    End Class

    ''' <summary>
    ''' 这个自定义属性与<see cref="DescriptionAttribute"/>的使用类似
    ''' 只不过<see cref="DescriptionAttribute"/>是针对命令的简单说明
    ''' 以及描述
    ''' 
    ''' 这个自定义属性是针对命令内的某些注意事项的更加详细的描述，默认是不显示的
    ''' 会在man模式下被显示出来
    ''' </summary>
    <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)>
    Public Class NoteAttribute : Inherits Attribute

        Public ReadOnly Property noteText As String

        Sub New(note As String)
            Me.noteText = note
        End Sub
    End Class
End Namespace
