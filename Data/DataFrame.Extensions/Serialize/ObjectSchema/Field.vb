﻿#Region "Microsoft.VisualBasic::c1e22f783d01499e74ec20d2bcbc8e5d, Data\DataFrame.Extensions\Field.vb"

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

    ' Class Field
    ' 
    '     Properties: Binding, BindProperty, InnerClass, Name, Type
    ' 
    '     Function: GetValue, ToString
    ' 
    ' Class [Class]
    ' 
    '     Properties: Fields, Stack, Type
    ' 
    '     Function: GetEnumerator, GetField, (+2 Overloads) GetSchema, IEnumerable_GetEnumerator, ToString
    ' 
    '     Sub: Remove
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Data.csv.StorageProvider
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection

Namespace Serialize.ObjectSchema

    ''' <summary>
    ''' + ``#`` uid;
    ''' + ``[FiledName]`` This field links to a external file, and id is point to the ``#`` uid field in the external file.
    ''' </summary>
    Public Class Field : Implements IReadOnlyId

        ''' <summary>
        ''' Field Name
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Name As String Implements IReadOnlyId.Identity
            Get
                Return Binding.Name
            End Get
        End Property

        Public ReadOnly Property BindProperty As PropertyInfo
            Get
                Return Binding.BindProperty
            End Get
        End Property

        Public ReadOnly Property Type As Type
            Get
                Return BindProperty.PropertyType
            End Get
        End Property

        ''' <summary>
        ''' 首先DirectCast为<see cref="IAttributeComponent"/>类型
        ''' </summary>
        ''' <returns></returns>
        Public Property Binding As ComponentModels.StorageProvider
        ''' <summary>
        ''' 假若这个为Nothing，则说明当前的域是简单类型
        ''' </summary>
        ''' <returns></returns>
        Public Property InnerClass As [Class]

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetValue(x As Object) As Object
            Return Binding.BindProperty.GetValue(x, Nothing)
        End Function

        Public Overrides Function ToString() As String
            Return Binding.ToString
        End Function
    End Class
End Namespace