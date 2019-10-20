///////////////////////////////////////////////////////////
//  Mask.cs
//  Implementation of the Class Mask
//  Generated by Enterprise Architect
//  Created on:      20-���-2019 10:46:17
//  Original author: Heln
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Domain
{
	public class Mask : IPixelContainer
    {

        public List<Pixel> Pixels { get; set; }
        public Pixel CentralRixel { get; set; } 
		public Pixel SymmetryCenter { get; set; } 

		public Mask()
        {

		}

		public int Sum()
        {

			return 0;
		}

    }

}