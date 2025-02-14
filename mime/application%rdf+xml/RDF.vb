﻿#Region "Microsoft.VisualBasic::e6efa560946179068e820fadc6c5e0e3, mime\application%rdf+xml\RDF.vb"

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

    ' Class RDF
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: (+2 Overloads) LoadDocument
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Xml.Serialization

Public Class RDF

    ''' <summary>
    ''' rdf:XXX
    ''' </summary>
    Public Const XmlnsNamespace$ = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"

    <XmlNamespaceDeclarations()>
    Public xmlns As XmlSerializerNamespaces

    Sub New()
        xmlns.Add("rdf", RDF.XmlnsNamespace)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Text">参数值为文件之中的字符串内容，而非文件的路径</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function LoadDocument(Text As String) As RDF
        Throw New NotImplementedException
    End Function

    Public Shared Function LoadDocument(Of T As RDF)(path As String) As T
        Return path.LoadXml(Of T)()
    End Function
End Class
