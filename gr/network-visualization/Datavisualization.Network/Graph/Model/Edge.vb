﻿#Region "Microsoft.VisualBasic::cab6b911f7ff1e7bad10b633b7db1f62, gr\network-visualization\Datavisualization.Network\Graph\Model\Edge.vb"

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

    '     Class Edge
    ' 
    '         Properties: __source, __target, data, ID, isDirected
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: Clone, (+2 Overloads) Equals, GetHashCode, Iterate2Nodes, ToString
    '         Operators: <>, =
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'! 
'@file Edge.cs
'@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
'		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
'@date August 08, 2013
'@brief Edge Interface
'@version 1.0
'
'@section LICENSE
'
'The MIT License (MIT)
'
'Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>
'
'Permission is hereby granted, free of charge, to any person obtaining a copy
'of this software and associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
'copies of the Software, and to permit persons to whom the Software is
'furnished to do so, subject to the following conditions:
'
'The above copyright notice and this permission notice shall be included in
'all copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
'LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
'OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
'THE SOFTWARE.
'
'@section DESCRIPTION
'
'An Interface for the Graph Class.
'
'

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph.Abstract
Imports Microsoft.VisualBasic.Serialization

Namespace Graph

    Public Class Edge : Inherits GraphTheory.Network.Edge(Of Node)
        Implements IInteraction
        Implements IGraphValueContainer(Of EdgeData)
        Implements ICloneable(Of Edge)

        Dim uniqueID As String = Nothing

        ''' <summary>
        ''' 如果什么也不赋值，则默认自动根据node编号来生成唯一id
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Property ID As String
            Get
                If uniqueID.StringEmpty Then
                    Return MyBase.ID
                Else
                    Return uniqueID
                End If
            End Get
            Set(value As String)
                uniqueID = value
            End Set
        End Property

        Public Property data As EdgeData Implements Selector.IGraphValueContainer(Of EdgeData).data
        Public Property isDirected As Boolean

#Region "Implements IInteraction"
        Private Property __source As String Implements IInteraction.source
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return U.label
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Private Property __target As String Implements IInteraction.target
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return V.label
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property
#End Region

        Public Sub New(id As String, source As Node, target As Node, Optional data As EdgeData = Nothing)
            Me.ID = id
            Me.data = If(data, New EdgeData())

            U = source
            V = target
            isDirected = False
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New()
            Call Me.New(Nothing, Nothing, Nothing, Nothing)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return ID
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function GetHashCode() As Integer
            Return ID.GetHashCode()
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            ' If parameter is null return false.
            If obj Is Nothing Then
                Return False
            Else
                Return Equals(p:=TryCast(obj, Edge))
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overloads Function Equals(p As Edge) As Boolean
            ' If parameter is null return false:
            If p Is Nothing Then
                Return False
            Else
                ' Return true if the fields match:
                Return (ID = p.ID)
            End If
        End Function

        Public Shared Operator =(a As Edge, b As Edge) As Boolean
            ' If both are null, or both are same instance, return true.
            If Object.ReferenceEquals(a, b) Then
                Return True
            End If

            ' If one is null, but not both, return false.
            If (DirectCast(a, Object) Is Nothing) OrElse (DirectCast(b, Object) Is Nothing) Then
                Return False
            End If

            ' Return true if the fields match:
            Return a.ID = b.ID
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator <>(a As Edge, b As Edge) As Boolean
            Return Not (a = b)
        End Operator

        Public Iterator Function Iterate2Nodes() As IEnumerable(Of Node)
            Yield U
            Yield V
        End Function

        Public Function Clone() As Edge Implements ICloneable(Of Edge).Clone
            Return New Edge With {
                .ID = ID,
                .isDirected = isDirected,
                .U = U,
                .V = V,
                .weight = weight,
                .data = New EdgeData With {
                    .label = data.label,
                    .length = data.length,
                    .weight = data.weight,
                    .Properties = New Dictionary(Of String, String)(data.Properties)
                }
            }
        End Function
    End Class
End Namespace
