init
{
	$SODX 65 66 67 68 69 256 82 83 264 272;
	// 256 -  Thickness 1, 264 -  Thickness 2, 272 - Thickness 3, 83 Sample Counter, 82 Intensity, 65-69 - Encoder
}

fn main(scanFreq= 50000) //Frequency the mirror speed is based on, should for must purposes be the same as the SHZ setting of the CHRocodile
{
	setFrequency(50000)
	exposure(50000, 50)	//50 KHz, LAI 50
	dwd{				//DWD to exclude the peak around 0. Up to 16 blocks of DWD can be added
		add(20,1500, 1)
	}
	rect(x0=-24.53,y0=-15.16,x1=25.47,y1=15.23,nCols=500,nRows=303,interp=1,waitAtBegin=15000,waitAtEnd=250,label="AreaScan 1")
	//Scan Area is defined by the top left corner (x0, y0) and the bottom right corner of the coordinate system 
}
