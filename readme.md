# Pointer Searcher
created by Takumi4685.

## warning
* this tool is alpha version,and may has many bugs.  
* this tool uses huge amout of memory,maybe more than 400 MB.
* each process is too slow.  
* I'm not going to fix bug,improve memory usage and calculating speed,enhancement.  
so don't request to me.

## how to use
### preparing
1. dump all r/w memory by using noexs and search non static address(target address you want to make a pointer code).
1. memo main start address,main end address,heap start address, heap end address and target address.  
it's better to restart game,and dump another data and memo to narrow down pointer codes.

### search pointer
1. launch pointer searcher
1. fill out 1st row
1. select `Read 1st Dump Data` button  
this process will takes 10-20 min or more  
`Reset and Search` button will be enabled when process finished
1. After setting search option below,select `Read 1st Dump Data` button  
Max Depth : Max pointer depth  
Offset Num : Num of address,which is pointed from another address,around searching address  
Offset Range : Max offset range  
calculating cost will be increase by Max Depth power of Offset Num
1. If too many results,get another dump data and fill out 2nd and subsequent row,select `Narrow Down Result`

# link
* tutorial(Japanese)  
https://zit866.hatenablog.com/entry/2019/12/17/012933

# credit
* Matthew Bell : auther of great tool ,Noexes.
* ZiT866 : thanks to making tutorial.

# change log
*v0.02*  
[bugfix]can't find pointer that includes +00h offset  
[improve]1st read become faster  

*v0.01*  
    first release