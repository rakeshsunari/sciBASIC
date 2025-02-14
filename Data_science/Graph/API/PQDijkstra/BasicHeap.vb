﻿#Region "Microsoft.VisualBasic::1426d0576106690256e975422c591c9d, Data_science\Graph\API\PQDijkstra\BasicHeap.vb"

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

    '     Class BasicHeap
    ' 
    '         Properties: Count
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: Pop
    ' 
    '         Sub: Push
    ' 
    ' 
    ' /********************************************************************************/

#End Region

'by Tolga Birdal

Imports System.Collections
Imports System.Collections.Generic
Imports System.Text

Namespace Dijkstra.PQDijkstra

    Public Class BasicHeap

        Dim InnerList As New List(Of HeapNode)()

        Public Sub New()
        End Sub

        Public ReadOnly Property Count() As Integer
            Get
                Return InnerList.Count
            End Get
        End Property

        Public Sub Push(index As Integer, weight As Single)
            Dim p As Integer = InnerList.Count, p2 As Integer
            InnerList.Add(New HeapNode(index, weight))
            ' E[p] = O
            Do
                If p = 0 Then
                    Exit Do
                End If

                p2 = (p - 1) >> 1
                If InnerList(p).weight < InnerList(p2).weight Then
                    Dim h As HeapNode = InnerList(p)
                    InnerList(p) = InnerList(p2)
                    InnerList(p2) = h

                    p = p2
                Else
                    Exit Do
                End If
            Loop While True
        End Sub

        Public Function Pop() As Integer
            Dim result As HeapNode = InnerList(0)
            Dim p As Integer = 0, p1 As Integer, p2 As Integer, pn As Integer
            InnerList(0) = InnerList(InnerList.Count - 1)
            InnerList.RemoveAt(InnerList.Count - 1)
            Do
                pn = p
                p1 = (p << 1) + 1
                p2 = (p << 1) + 2
                If InnerList.Count > p1 AndAlso InnerList(p).weight > InnerList(p1).weight Then
                    p = p1
                End If
                If InnerList.Count > p2 AndAlso InnerList(p).weight > InnerList(p2).weight Then
                    p = p2
                End If

                If p = pn Then
                    Exit Do
                End If

                Dim h As HeapNode = InnerList(p)
                InnerList(p) = InnerList(pn)

                InnerList(pn) = h
            Loop While True
            Return result.index
        End Function
    End Class
End Namespace
