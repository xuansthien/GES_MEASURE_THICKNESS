init
{
	$OFN 0 1;
	$SODX 65 66 67 68 69 82 83 256 257 264 265 272 273 280 281 288 289 296 297 304 305 312 313;
	// 256 -  Thickness 1, 264 -  Thickness 2, 272 - Thickness 3, 83 Sample Counter, 82 Intensity, 65-69 - Encoder
}

fn main(scanFreq= 50000) //Frequency the mirror speed is based on, should for must purposes be the same as the SHZ setting of the CHRocodile
{
	setFrequency(50000)
	exposure(50000, 50)	//50 KHz, LAI 50
	rect(x0=-8,y0=-34,x1=8,y1=34,nCols=800,nRows=3400,interp=0,waitAtBegin=15000,waitAtEnd=250,label="AreaScan 1")
}
