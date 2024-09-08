# 简介  
某大学简称为“高程”的课程使用  
完成测试后在本应用建立的目录下一键生成（字符串升序排列）：  
* 项目信息记录文件（__project.gaocheng）  
* 编号后的测试数据（__test_data.txt）  
* 比对结果（_compare_result.txt）  
* demo测试结果（_demo_exe_result.txt）  
* 作业exe测试结果（_your_exe_result.txt）  
* 测试批处理（test.bat）  

## 界面预览（适用于1.6.1及之后）  
<ul>
  <li>应用主界面预览<br>
    <img alt="主界面预览图片" src="./img/MainForm.png" width="640px" height="357px"></li>
  <li>单次测试生成的所有文件预览<br>
    <img alt="单个项目生成文件预览" src="./img/ProjectFiles.png" width="640px" height="249px"></li>
</ul>

## 编译须知  
首次编译后直接运行可能报错，需要将resource文件夹中的的rsc文件夹以及README.html文件复制到生成exe同级目录下。若还无法正常运行，则检查MutSync.cs文件代码中检查的文件存在性、检查的文件大小是否超过2MB，以及MD5是否正确

<blockquote>（“你一定是坚坚的学生吧”<br>
<a href="https://www.zhihu.com/question/554569818/answer/2683685957" target="_blank">——YouKnowWho from 某蓝色高端论坛</a>
</blockquote>