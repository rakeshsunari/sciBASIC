﻿#Region "Microsoft.VisualBasic::ad51791b4b463fdd6569af2b93135f77, Data_science\Mathematica\Math\Math\Algebra\Vector\Polynomial.vb"

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

    '     Class Polynomial
    ' 
    '         Properties: Factors, IsLinear
    ' 
    '         Function: (+3 Overloads) ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace LinearAlgebra

    Public Class Polynomial

        ''' <summary>
        ''' 多项式系数向量
        ''' </summary>
        ''' <returns></returns>
        Public Property Factors As Double()

        Default Public ReadOnly Property F(x#) As Double
            Get
                Dim ans As Double = 0

                For i As Integer = 0 To Factors.Length - 1
                    ans += Factors(i) * (x ^ i)
                Next

                Return ans
            End Get
        End Property

        Public ReadOnly Property IsLinear As Boolean
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Factors.Length <= 2
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return ToString(format:="F2")
        End Function

        Public Overloads Function ToString(format As String, Optional html As Boolean = False) As String
            Dim items = Factors _
                .Select(Function(a, i)
                            If i = 0 Then
                                Return ToString(a, format, html)
                            ElseIf i = 1 Then
                                Return $"{ToString(a, format, html)}*X"
                            Else
                                Return $"{ToString(a, format, html)}*X^{i}"
                            End If
                        End Function) _
                .ToArray
            Dim Y$ = items.JoinBy(" + ")

            Return Y
        End Function

        Private Overloads Shared Function ToString(a As Double, format$, html As Boolean) As String
            Dim text = a.ToString(format).ToLower

            If Not html Then
                Return text
            Else
                Dim t As String() = text.Split("e"c)

                If t.Length = 1 Then
                    Return text
                Else
                    Return $"{t(Scan0)}e<sup>{t(1)}</sup>"
                End If
            End If
        End Function
    End Class
End Namespace
