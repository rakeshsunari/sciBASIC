﻿#Region "Microsoft.VisualBasic::ddeeb7ad1a7ce33ee9840b24595558b1, Microsoft.VisualBasic.Core\Text\Xml\Linq\NodeIterator.vb"

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

    '     Module NodeIterator
    ' 
    '         Function: GetArrayTemplate, IterateArrayNodes
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Xml
Imports Microsoft.VisualBasic.Language

Namespace Text.Xml.Linq

    Public Module NodeIterator

        Friend Const XmlDeclare$ = "<?xml version=""1.0"" encoding=""utf-16""?>"
        Friend Const ArrayOfTemplate$ = XmlDeclare & "
<ArrayOf{0} xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
%s
</ArrayOf{0}>"

        ''' <summary>
        ''' 可以将模板文本之中的``%s``替换为相应的Xml数组文本
        ''' </summary>
        ''' <typeparam name="T">
        ''' 在.NET的XML序列化之中，数组元素的类型名称首字母会自动的被转换为大写形式
        ''' </typeparam>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetArrayTemplate(Of T As Class)() As String
            Return ArrayOfTemplate.Replace("{0}", GetType(T).GetNodeNameDefine.UpperCaseFirstChar)
        End Function

        ''' <summary>
        ''' 使用<see cref="XmlDocument.Load"/>方法加载XML文档依旧是一次性的全部加载所有的文本到内存之中，第一次加载效率会比较低
        ''' 则可以使用这个方法来加载非常大的XML文档
        ''' </summary>
        ''' <param name="path$"></param>
        ''' <returns></returns>
        <Extension> Public Iterator Function IterateArrayNodes(path$, tag$, Optional filter As Func(Of String, Boolean) = Nothing) As IEnumerable(Of String)
            Dim buffer As New List(Of String)
            Dim start$ = "<" & tag
            Dim ends$ = $"</{tag}>"
            Dim stack%
            Dim tagOpen As Boolean = False
            Dim lefts$
            Dim i%
            Dim xmlText$

            For Each line As String In path.IterateAllLines
                If tagOpen Then

                    i = InStr(line, ends)

                    If i > 0 Then
                        ' 遇到了结束标签，则取出来
                        If stack > 0 Then
                            stack -= 1 ' 内部栈，还没有结束，则忽略当前的这个标签
                        Else
                            ' 这个是真正的结束标签
                            lefts = Mid(line, i + ends.Length)
                            buffer += ends
                            tagOpen = False

                            xmlText = buffer.JoinBy(vbLf)
                            buffer *= 0
                            buffer += lefts

                            If Not filter Is Nothing AndAlso filter(xmlText) Then
                                ' skip
                            Else
                                ' populate data
                                Yield xmlText
                            End If

                            ' 这里要跳出来，否则后面buffer += line处任然会添加这个结束标签行的
                            Continue For
                        End If
                    ElseIf InStr(line, start) > 0 Then
                        stack += 1
                    End If

                    buffer += line
                Else
                    ' 需要一直遍历到开始标签为止
                    i = InStr(line, start)

                    If i > 0 Then
                        tagOpen = True
                        buffer += Mid(line, i)
                    End If
                End If
            Next
        End Function
    End Module
End Namespace
