# WebApi:
# 项目整体依赖接口编程，共分为IRepository仓储接口层，Repository仓储实现层，IService业务接口层,Service业务实现层，EFModel数据实体类库，ViewModel展示实体类库，Common公共方法类库，Infrastructure基础设施层，WebApi提供外部调用接口
# 使用AutoFac作为依赖注入容器解耦依赖，同时管理数据上下文每次请求唯一，和每次会话唯一
# 提供Redis，Memcache，HttpRuntime.Cache三种方式作为缓存
# 实现Aop 异常捕捉并使用redis.List作为队列记录异常日志
# 使用swagger方便接口测试调用
