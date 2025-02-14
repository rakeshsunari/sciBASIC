﻿#Region "Microsoft.VisualBasic::e5ae64f845b53aa6957e99cc714b1792, Microsoft.VisualBasic.Core\Scripting\MetaData\Type.vb"

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

    '     Class TypeInfo
    ' 
    '         Properties: assembly, fullName, isSystemKnownType
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: [GetType], LoadAssembly, ToString
    ' 
    '         Sub: doInfoParser
    ' 
    '         Operators: <>, =
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Language.Default

Namespace Scripting.MetaData

    ''' <summary>
    ''' The type reference information.(类型信息)
    ''' </summary>
    Public Class TypeInfo

        ''' <summary>
        ''' The assembly file which contains this type definition.(模块文件)
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute> Public Property assembly As String
        ''' <summary>
        ''' <see cref="Type.FullName"/>.(类型源)
        ''' </summary>
        ''' <returns></returns>
        <XmlText> Public Property fullName As String

        ''' <summary>
        ''' Is this type object is a known system type?(是否是已知的类型？)
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property isSystemKnownType As Boolean
            Get
                Return Not Scripting.GetType(fullName) Is Nothing
            End Get
        End Property

        Sub New()
        End Sub

        ''' <summary>
        ''' Creates type reference from the definition.
        ''' </summary>
        ''' <param name="info"></param>
        Sub New(info As Type)
            Call doInfoParser(info, assembly, fullName)
        End Sub

        Private Shared Sub doInfoParser(info As Type, ByRef assm As String, ByRef id As String)
            assm = info.Assembly.Location.FileName
            id = info.FullName
        End Sub

        Public Overrides Function ToString() As String
            Return $"{assembly}!{fullName}"
        End Function

        ''' <summary>
        ''' Loads the assembly file which contains this type. If the <param name="directory"></param> is not a valid directory location, 
        ''' then using the location <see cref="App.HOME"/> as default.
        ''' </summary>
        ''' <returns></returns>
        Public Function LoadAssembly(Optional directory As DefaultString = Nothing) As Assembly
            Dim path As String = $"{directory Or App.HOME}/{Me.assembly}"
            Dim assm As Assembly = System.Reflection.Assembly.LoadFile(path)

            Return assm
        End Function

        ''' <summary>
        ''' Get mapping type information.
        ''' </summary>
        ''' <param name="knownFirst">
        ''' 如果这个参数为真的话, 则会尝试直接从当前的应用程序域中查找类信息, 反之则会加载目标程序集进行类型信息查找
        ''' </param>
        ''' <param name="throwEx">
        ''' 如果这个参数设置为False的话，则出错的时候会返回空值
        ''' </param>
        ''' <returns></returns>
        Public Overloads Function [GetType](Optional knownFirst As Boolean = False,
                                            Optional throwEx As Boolean = True,
                                            Optional ByRef getException As Exception = Nothing) As Type
            Dim type As Type
            Dim assm As Assembly

            If knownFirst Then
                type = Scripting.GetType(fullName)

                If Not type Is Nothing Then
                    Return type
                End If
            End If

            ' 错误一般出现在loadassembly阶段
            ' 主要是文件未找到
            Try
                assm = LoadAssembly()
                type = assm.GetType(Me.fullName)
                getException = Nothing
            Catch ex As Exception
                ex = New DllNotFoundException(ToString, ex)

                If throwEx Then
                    Throw ex
                Else
                    getException = ex
                End If
            Finally
                type = Nothing
            End Try

            Return type
        End Function

        ''' <summary>
        ''' 检查a是否是指向b的类型引用的
        ''' </summary>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <returns></returns>
        Public Overloads Shared Operator =(a As TypeInfo, b As Type) As Boolean
            Dim assm As String = Nothing, type As String = Nothing
            Call doInfoParser(b, assm, type)
            Return String.Equals(a.assembly, assm, StringComparison.OrdinalIgnoreCase) AndAlso
                String.Equals(a.fullName, type, StringComparison.Ordinal)
        End Operator

        Public Overloads Shared Operator <>(a As TypeInfo, b As Type) As Boolean
            Return Not a = b
        End Operator
    End Class
End Namespace
