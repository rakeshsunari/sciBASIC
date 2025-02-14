﻿#Region "Microsoft.VisualBasic::648f753158ccea09eef1164f36ba0dd6, Data_science\Mathematica\Math\Math.Statistics\Distributions\MethodOfMoments\Gumbel.vb"

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

    '     Class Gumbel
    ' 
    '         Constructor: (+3 Overloads) Sub New
    '         Function: GetCDF, GetInvCDF, GetPDF, Validate
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports System.Collections.Generic

'
' * To change this license header, choose License Headers in Project Properties.
' * To change this template file, choose Tools | Templates
' * and open the template in the editor.
' 
Namespace Distributions.MethodOfMoments


    ''' 
    ''' <summary>
    ''' @author Will_and_Sara
    ''' </summary>
    Public Class Gumbel
        Inherits Distributions.ContinuousDistribution

        Private _Mu As Double
        Private _Beta As Double
        Public Sub New()
            _Mu = 0
            _Beta = 0
        End Sub
        Public Sub New(mu As Double, beta As Double)
            _Mu = mu
            _Beta = beta
        End Sub
        Public Sub New(data As Double())
            Dim BPM As New MomentFunctions.BasicProductMoments(data)
            _Beta = Math.PI / (BPM.StDev() * Math.Sqrt(6))
            _Mu = BPM.Mean() - _Beta * 0.57721566490153287
            PeriodOfRecord = (BPM.SampleSize())
        End Sub
        Public Overrides Function GetInvCDF(probability As Double) As Double
            Return (_Mu - (_Beta * (Math.Log(Math.Log(probability)))))
        End Function
        Public Overrides Function GetCDF(value As Double) As Double
            Return Math.Exp(-Math.Exp(-(value - _Mu) / _Beta))
        End Function
        Public Overrides Function GetPDF(value As Double) As Double
            Dim z As Double = (value - _Mu) / _Beta
            Return (1 / _Beta) * Math.Exp(-(z + Math.Exp(-z)))
        End Function
        Public Overrides Iterator Function Validate() As IEnumerable(Of Exception)
            If _Beta <= 0 Then Yield New Exception("Beta must be greater than 0")
        End Function
    End Class

End Namespace
