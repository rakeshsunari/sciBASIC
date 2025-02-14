﻿#Region "Microsoft.VisualBasic::53ee1938cc549f8ae6442f02659fabb9, Microsoft.VisualBasic.Core\Extensions\Math\StatisticsMathExtensions\Linq\EnumerableStatsVariance.vb"

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

    '     Module EnumerableStatsVariance
    ' 
    '         Function: (+20 Overloads) Variance
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports sys = System.Math

Namespace Math.Statistics.Linq

    Public Module EnumerableStatsVariance

        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Decimal values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Decimal values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Decimal?)) As Decimal
            Dim values As IEnumerable(Of Decimal) = source.Coalesce()
            If values.Any() Then
                Return values.Variance()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Decimal values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Decimal values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Decimal)) As Decimal
            Return CDec(source.[Select](Function(x) CDbl(x)).Variance())
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Double values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Double values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Double?)) As Double
            Dim values As IEnumerable(Of Double) = source.Coalesce()
            If values.Any() Then
                Return values.Variance()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Double values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Double values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Double)) As Double
            Dim avg As Double = source.Average()
            Dim d As Double = source.Aggregate(0.0, Function(total, [next]) As Double
                                                        total += sys.Pow([next] - avg, 2)
                                                        Return total
                                                    End Function)
            Return d / (source.Count() - 1)
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Single values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Single values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Single?)) As Single
            Dim values As IEnumerable(Of Single) = source.Coalesce()
            If values.Any() Then
                Return values.Variance()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Single values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Single values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Single)) As Single
            Return CSng(source.[Select](Function(x) CDbl(x)).Variance())
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Int32 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Int32values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Integer?)) As Double
            Dim values As IEnumerable(Of Integer) = source.Coalesce()
            If values.Any() Then
                Return values.Variance()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Int32 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Int32 values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Integer)) As Double
            Return source.[Select](Function(x) CDbl(x)).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Int64 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of nullable System.Int64 values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Long?)) As Double
            Dim values As IEnumerable(Of Long) = source.Coalesce()
            If values.Any() Then
                Return values.Variance()
            End If

            Return Nothing
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Int64 values.
        '
        ' Parameters:
        '   source:
        '     A sequence of System.Int64 values to calculate the Variance of.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(source As IEnumerable(Of Long)) As Double
            Return source.[Select](Function(x) CDbl(x)).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Decimal values that
        '     are obtained by invoking a transform function on each element of the input
        '     sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Decimal?)) As Decimal
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Decimal values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values that are used to calculate an Variance.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Decimal.MaxValue.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Decimal)) As Decimal
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Double values that
        '     are obtained by invoking a transform function on each element of the input
        '     sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Double?)) As Double
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Double values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Double)) As Double
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Single values that
        '     are obtained by invoking a transform function on each element of the input
        '     sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Single?)) As Single
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Single values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Single)) As Single
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Int32 values that are
        '     obtained by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Integer?)) As Double
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Int32 values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Integer)) As Double
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of nullable System.Int64 values that are
        '     obtained by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values, or null if the source sequence is
        '     empty or contains only values that are null.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Long?)) As Double
            Return source.[Select](selector).Variance()
        End Function
        '
        ' Summary:
        '     Computes the Variance of a sequence of System.Int64 values that are obtained
        '     by invoking a transform function on each element of the input sequence.
        '
        ' Parameters:
        '   source:
        '     A sequence of values to calculate the Variance of.
        '
        '   selector:
        '     A transform function to apply to each element.
        '
        ' Type parameters:
        '   TSource:
        '     The type of the elements of source.
        '
        ' Returns:
        '     The Variance of the sequence of values.
        '
        ' Exceptions:
        '   System.ArgumentNullException:
        '     source or selector is null.
        '
        '   System.InvalidOperationException:
        '     source contains no elements.
        '
        '   System.OverflowException:
        '     The sum of the elements in the sequence is larger than System.Int64.MaxValue.
        <Extension>
        Public Function Variance(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Long)) As Double
            Return source.[Select](selector).Variance()
        End Function
    End Module
End Namespace
