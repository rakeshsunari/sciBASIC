﻿#Region "Microsoft.VisualBasic::34463f5b11d7217102871b5f64a86aac, Data_science\Mathematica\Math\DataFrame\Correlation.vb"

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

    ' Module Correlation
    ' 
    '     Function: CorrelatesNormalized, CorrelationMatrix, Pearson, Spearman
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Correlations.Correlations

Public Module Correlation

    ''' <summary>
    ''' 这个函数是计算列之间的相关度的
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Function CorrelationMatrix(data As IEnumerable(Of DataSet), Optional doCor As ICorrelation = Nothing) As IEnumerable(Of DataSet)
        Dim dataset As DataSet() = data.ToArray
        Dim columns = dataset.PropertyNames _
            .Select(Function(colName)
                        Return New NamedValue(Of Double()) With {
                            .Name = colName,
                            .Value = dataset _
                                .Select(Function(d) d(colName)) _
                                .ToArray
                        }
                    End Function) _
            .ToArray

        Return Correlations.CorrelationMatrix(columns, doCor) _
            .Select(Function(r)
                        Return New DataSet With {
                            .ID = r.Name,
                            .Properties = r.Value
                        }
                    End Function)
    End Function

    ''' <summary>
    ''' 这个函数处理的是没有经过归一化处理的原始数据(这个函数是计算行之间的相关度的)
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="doCor">假若这个参数为空，则默认使用<see cref="Correlations.GetPearson(Double(), Double())"/></param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function CorrelatesNormalized(data As IEnumerable(Of DataSet), Optional doCor As ICorrelation = Nothing) As IEnumerable(Of NamedValue(Of Dictionary(Of String, Double)))
        Dim dataset As DataSet() = data.ToArray
        Dim keys$() = dataset(Scan0) _
            .Properties _
            .Keys _
            .ToArray
        Dim b As Double()

        doCor = doCor Or PearsonDefault

        For Each x As DataSet In dataset
            Dim out As New Dictionary(Of String, Double)
            Dim array#() = keys _
                .Select(Of Double)(x) _
                .ToArray

            For Each y As DataSet In dataset
                b = keys.Select(Of Double)(y).ToArray
                out(y.ID) = doCor(X:=array, Y:=b)
            Next

            Yield New NamedValue(Of Dictionary(Of String, Double)) With {
                .Name = x.ID,
                .Value = out
            }
        Next
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function Pearson(data As IEnumerable(Of DataSet), Optional visit As MatrixVisit = MatrixVisit.ByRow) As IEnumerable(Of DataSet)
        If visit = MatrixVisit.ByRow Then
            Return data.CorrelatesNormalized(AddressOf GetPearson) _
                .Select(Function(r)
                            Return New DataSet With {
                                .ID = r.Name,
                                .Properties = r.Value
                            }
                        End Function)
        Else
            Return data.CorrelationMatrix(AddressOf GetPearson)
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function Spearman(data As IEnumerable(Of DataSet), Optional visit As MatrixVisit = MatrixVisit.ByRow) As IEnumerable(Of DataSet)
        If visit = MatrixVisit.ByRow Then
            Return data.CorrelatesNormalized(AddressOf Correlations.Spearman) _
                .Select(Function(r)
                            Return New DataSet With {
                                .ID = r.Name,
                                .Properties = r.Value
                            }
                        End Function)
        Else
            Return data.CorrelationMatrix(AddressOf Correlations.Spearman)
        End If
    End Function
End Module
