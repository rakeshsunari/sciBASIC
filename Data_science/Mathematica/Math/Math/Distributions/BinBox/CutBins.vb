﻿#Region "Microsoft.VisualBasic::27a36f04fdcce248a14aba1ecb91cc5b, Data_science\Mathematica\Math\Math\Distributions\BinBox\CutBins.vb"

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

    '     Module CutBins
    ' 
    '         Function: EqualFrequencyBins, (+3 Overloads) FixedWidthBins
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Language

Namespace Distributions.BinBox

    ''' <summary>
    ''' 进行数据分箱操作
    ''' </summary>
    Public Module CutBins

        ''' <summary>
        ''' ### 数据等宽分箱
        ''' 
        ''' 将变量的取值范围分为<paramref name="k"/>个等宽的区间，每个区间当作一个分箱。
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 宽度是自动计算的
        ''' </remarks>
        Public Function FixedWidthBins(Of T)(data As IEnumerable(Of T), k%, eval As Evaluate(Of T)) As IEnumerable(Of DataBinBox(Of T))
            ' 升序排序方便进行快速计算
            Dim v = data.OrderBy(Function(d) eval(d)).ToArray
            Dim min# = eval(v.First)
            Dim max# = eval(v.Last)
            Dim width# = (max - min) / k

            Return FixedWidthBins(v, width, eval, min, max)
        End Function

        ''' <summary>
        ''' ### 数据等宽分箱
        ''' 
        ''' 将变量按照给定的值域宽度分为多个区间
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="v"></param>
        ''' <param name="width">所给定的区间宽度</param>
        ''' <param name="eval"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 宽度是手工指定的
        ''' </remarks>
        Public Function FixedWidthBins(Of T)(v As IEnumerable(Of T), width#, range As DoubleRange, eval As Evaluate(Of T)) As IEnumerable(Of DataBinBox(Of T))
            Return FixedWidthBins(v.OrderBy(Function(d) eval(d)).ToArray, width, eval, range.Min, range.Max)
        End Function

        ''' <summary>
        ''' ### 数据等宽分箱
        ''' 
        ''' 将变量按照给定的值域宽度分为多个区间
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="v"></param>
        ''' <param name="width">所给定的区间宽度</param>
        ''' <param name="eval"></param>
        ''' <returns></returns>
        Public Iterator Function FixedWidthBins(Of T)(v As T(), width#, eval As Evaluate(Of T), min#, max#, Optional slideWindowSteps# = -1) As IEnumerable(Of DataBinBox(Of T))
            Dim x As New Value(Of Double)
            Dim len% = v.Length
            Dim i As i32 = 0
            Dim lowerbound# = min
            Dim upbound#
            Dim list As New List(Of T)

            slideWindowSteps = slideWindowSteps Or width.When(slideWindowSteps <= 0)

            Do While lowerbound < max
                upbound = lowerbound + width

                ' 因为数据已经是经过排序了的，所以在这里可以直接进行区间计数
                Do While i < len AndAlso (x = eval(v(i))) >= lowerbound AndAlso x < upbound
                    list += v(++i)
                Loop

                Yield New DataBinBox(Of T)(list, eval)

                If i.Value = len Then
                    Exit Do
                Else
                    list *= 0
                    lowerbound += slideWindowSteps
                End If
            Loop
        End Function

        ''' <summary>
        ''' 等频分箱，每一个bin之中的元素数目相等
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="k">得到K个bin</param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function EqualFrequencyBins(Of T)(data As IEnumerable(Of T), k%, eval As Evaluate(Of T)) As IEnumerable(Of DataBinBox(Of T))
            Dim v As T() = data _
                .OrderBy(Function(x) eval(x)) _
                .ToArray
            ' 计算出每一个bin之中的元素数目
            Dim size% = v.Length / k

            Return v _
                .SplitIterator(size) _
                .Select(Function(block)
                            Return New DataBinBox(Of T)(block, eval)
                        End Function)
        End Function
    End Module
End Namespace
