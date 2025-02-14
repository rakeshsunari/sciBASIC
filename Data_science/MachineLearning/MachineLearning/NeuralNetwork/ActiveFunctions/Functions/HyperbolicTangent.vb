﻿#Region "Microsoft.VisualBasic::f6c3216ce47751b8d0e03cdc2968ecfa, Data_science\MachineLearning\MachineLearning\NeuralNetwork\ActiveFunctions\Functions\HyperbolicTangent.vb"

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

    '     Class HyperbolicTangent
    ' 
    '         Properties: Store
    ' 
    '         Function: [Function], Derivative, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.StoreProcedure
Imports sys = System.Math

Namespace NeuralNetwork.Activations

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' ```
    '''         e ^ x - e ^ -x
    ''' f(x) = -----------------
    '''         e ^ x + e ^ -x
    ''' 
    ''' ```
    ''' </remarks>
    <Serializable>
    Public Class HyperbolicTangent : Inherits IActivationFunction

        Public Overrides ReadOnly Property Store As ActiveFunction
            Get
                Return New ActiveFunction() With {
                    .Arguments = {},
                    .name = NameOf(HyperbolicTangent)
                }
            End Get
        End Property

        ''' <summary>
        ''' 这个函数接受的参数应该是一个弧度值
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        Public Overrides Function [Function](x As Double) As Double
            Dim a = Math.E ^ x
            Dim b = Math.E ^ (-x)

            Return (a - b) / (a + b)
        End Function

        ''' <summary>
        ''' 这个函数所接受的参数也是一个弧度值
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Protected Overrides Function Derivative(x As Double) As Double
            Return 1 / (sys.Cosh(x) ^ 2)
        End Function

        Public Overrides Function ToString() As String
            Return Store.ToString
        End Function
    End Class
End Namespace
