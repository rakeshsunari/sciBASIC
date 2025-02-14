﻿#Region "Microsoft.VisualBasic::279bc99b8a2f0e3142f9b9687a70183e, Data_science\DataMining\DataMining\ComponentModel\Evaluation\ROC.vb"

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

    '     Module ROC
    ' 
    '         Function: (+2 Overloads) AUC
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Correlations
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports stdNum = System.Math

Namespace ComponentModel.Evaluation

    ''' <summary>
    ''' The ROC math module
    ''' </summary>
    Public Module ROC

        ''' <summary>
        ''' 使用梯形面积法计算AUC的结果值
        ''' </summary>
        ''' <param name="validates"></param>
        ''' <returns></returns>
        <Extension>
        Public Function AUC(validates As IEnumerable(Of Validation)) As Double
            Dim data As Validation() = validates _
                .OrderByDescending(Function(d) d.Threshold) _
                .ToArray
            Dim accumulate = Iterator Function() As IEnumerable(Of Double)
                                 Dim x2, x1 As Double
                                 Dim fx2, fx1 As Double
                                 Dim h As Double

                                 ' x = 1 - Specificity
                                 ' y = Sensibility
                                 '
                                 ' 梯形面积计算： 矩形面积+直角三角形面积

                                 For i As Integer = 1 To data.Length - 1
                                     x2 = 100 - data(i).Specificity
                                     x1 = 100 - data(i - 1).Specificity
                                     fx2 = data(i).Sensibility
                                     fx1 = data(i).Sensibility
                                     h = x2 - x1

                                     ' 矩形面积 + 直角三角形面积
                                     Yield h * stdNum.Min(fx2, fx1) + (h * stdNum.Abs(fx2 - fx1)) / 2
                                 Next
                             End Function

            Return accumulate().Sum / 100
        End Function

        ''' <summary>
        ''' Rank排序法计算AUC面积
        ''' </summary>
        ''' <param name="validates"></param>
        ''' <returns></returns>
        <Extension>
        Public Iterator Function AUC(validates As IEnumerable(Of Validate), Optional names$() = Nothing) As IEnumerable(Of NamedValue(Of Double))
            Dim validateVector = validates.SafeQuery.ToArray
            Dim width% = validateVector(Scan0).width

            If names.IsNullOrEmpty Then
                names = width.SeqIterator _
                    .Select(Function(i) $"output_{i}") _
                    .ToArray
            End If

#Disable Warning
            For i As Integer = 0 To width - 1
                ' 首先对score从大到小排序
                Dim orderScoreDesc = validateVector _
                    .OrderByDescending(Function(test) test.predicts(i)) _
                    .ToArray
                ' 然后按照score进行ranking的计算
                Dim ranks = orderScoreDesc _
                    .Select(Function(test) test.predicts(i)) _
                    .Ranking(, desc:=False) _
                    .AsVector
                ' 然后把所有的正类样本的rank相加
                Dim positiveRankSum = Which _
                    .IsTrue(orderScoreDesc.Select(Function(test) test.actuals(i) > 0)) _
                    .DoCall(Function(indices) ranks(indices)) _
                    .Sum
                Dim M = orderScoreDesc.Count(Function(test) test.actuals(i) > 0)
                Dim N = validateVector.Length - M
                Dim aucValue = (positiveRankSum - M * (1 + M) / 2) / (M * N)

                Yield New NamedValue(Of Double) With {
                    .Name = names(i),
                    .Value = aucValue
                }
            Next
#Enable Warning
        End Function
    End Module
End Namespace
