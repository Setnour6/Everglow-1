﻿#define DEBUG
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

internal abstract class DPlatform : DynamicTile, IHookable
{
    private readonly Dictionary<(Direction, Direction), Direction> hitEdges = new Dictionary<(Direction, Direction), Direction>()
    {
        [(Direction.Top, Direction.Right)] = Direction.TopRight,
        [(Direction.Right, Direction.Bottom)] = Direction.BottomRight,
        [(Direction.Bottom, Direction.Left)] = Direction.BottomLeft,
        [(Direction.Top, Direction.Left)] = Direction.TopLeft,
        [(Direction.Top, Direction.Bottom)] = Direction.Inside,
        [(Direction.Right, Direction.Left)] = Direction.Inside,
        [(Direction.Top, Direction.None)] = Direction.Top,
        [(Direction.Right, Direction.None)] = Direction.Right,
        [(Direction.Bottom, Direction.None)] = Direction.Bottom,
        [(Direction.Left, Direction.None)] = Direction.Left,
    };
    public DPlatform()
    {

    }
    protected DPlatform(Vector2 position, Vector2 velocity, float width)
    {
        this.position = position;
        this.velocity = velocity;
        this.width = width;
    }
    protected DPlatform(Vector2 position, Vector2 velocity, float width, Rotation rotation, float miu)
    {
        this.position = position;
        this.velocity = velocity;
        this.width = width;
        this.rotation = rotation;
        this.miu = miu;
    }

    /// <summary>
    /// 宽度 
    /// </summary>
    public float width;
    /// <summary>
    /// 角度，面朝右为0
    /// </summary>
    public Rotation rotation;
    /// <summary>
    /// 转速
    /// </summary>
    public Rotation angularVelocity;
    /// <summary>
    /// 摩擦系数
    /// </summary>
    public float miu;
    /// <summary>
    /// 碰撞后可能站上去的加速度缓存
    /// </summary>
    private Vector2 cache;
    public Edge Edge => new Edge(position - rotation.YAxis * width / 2, position + rotation.YAxis * width / 2);
    public override Collider Collider => new CEdge(Edge);
    public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
    {
        if (ignorePlats)
        {
            return Direction.None;
        }

        var smallBox = aabb;
        smallBox.TopLeft += Vector2.One;
        smallBox.BottomRight -= Vector2.One;
        Edge edge = Edge;
        if (smallBox.Intersect(edge))
        {
            return Direction.None;
        }
        bool onGround = velocity.Y == 0;
        Vector2 rvel = move - this.velocity;
        Vector2 pos0 = aabb.position;
        Vector2 target = aabb.position + rvel;
        Direction result = Direction.None;
        if (Vector2.Dot(rotation.XAxis, aabb.Center - position) < 0)
        {
            return Direction.None;
        }
        aabb.position.Y = MathUtils.Approach(aabb.position.Y, target.Y, 1);
        Test.TestLog(velocity.Y);

        do
        {
            aabb.position = MathUtils.Approach(aabb.position, target, 1);
            if (edge.Intersect(aabb, out Array2<Direction> directions))
            {
                result = hitEdges[directions.Tuple];

                switch (result)
                {
                    //case Direction.Inside:
                    //    if (-MathHelper.PiOver4 * 3 < rotation.Angle && rotation.Angle <= -MathHelper.PiOver4 ||
                    //        (MathHelper.PiOver4 < rotation.Angle && rotation.Angle <= MathHelper.PiOver4 * 3))
                    //    {
                    //        result = aabb.Center.Y > position.Y ? Direction.Top : Direction.Bottom;
                    //    }
                    //    else
                    //    {
                    //        result = aabb.Center.X > position.X ? Direction.Left : Direction.Right;
                    //    }
                    //    break;
                    case Direction.Left:
                        if (!(aabb.Left.Distance(edge.begin.X) < 1) && !(aabb.Left.Distance(edge.end.X) < 1))
                        {
                            result = rotation.Angle < 0 ? Direction.Bottom : Direction.Top;
                        }
                        break;
                    case Direction.Right:
                        if (!(aabb.Right.Distance(edge.begin.X) < 1) && !(aabb.Right.Distance(edge.end.X) < 1))
                        {
                            result = rotation.Angle < 0 ? Direction.Bottom : Direction.Top;
                        }
                        break;
                    default:
                        break;
                }
                if (result == Direction.Inside)
                {
                    if (-MathHelper.PiOver4 * 3 < rotation.Angle && rotation.Angle <= -MathHelper.PiOver4 ||
                        (MathHelper.PiOver4 < rotation.Angle && rotation.Angle <= MathHelper.PiOver4 * 3))
                    {
                        result = aabb.Center.Y > position.Y ? Direction.Top : Direction.Bottom;
                        float w = (rotation.Angle < -MathHelper.PiOver2 ? aabb.Right : aabb.Left) - position.X;
                        float h = (rotation.Angle < -MathHelper.PiOver2 ? aabb.Right : aabb.Left) * edge.K + edge.B;
                        aabb.position.Y = h - (result == Direction.Bottom ? aabb.Height : 0);
                        aabb.position.X = target.X;
                        cache = angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
                        velocity.Y = this.velocity.Y + cache.Y;
                        velocity.Y = onGround && velocity.Y == 0 ?
                            Quick.AirSpeed :
                            velocity.Y;
                    }
                    else
                    {
                        result = aabb.Center.X > position.X ? Direction.Left : Direction.Right;
                    }
                }
                else if (result == Direction.Bottom)
                {
                    float h = (position.X < aabb.Center.X ? edge.end.Y : edge.begin.Y);
                    aabb.position.Y = h - aabb.Height;
                    aabb.position.X = target.X;
                    cache = angularVelocity.YAxis * width * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
                    velocity.Y = this.velocity.Y + cache.Y;
                    velocity.Y = onGround && velocity.Y == 0 ?
                        0 :
                        Quick.AirSpeed;
                }
                else if (result == Direction.Top)
                {
                    float h = (position.X > aabb.Center.X ? edge.end.Y : edge.begin.Y);
                    aabb.position.Y = h;
                    aabb.position.X = target.X;
                    cache = angularVelocity.YAxis * width * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
                    velocity.Y = this.velocity.Y + cache.Y;
                    velocity.Y = onGround && velocity.Y == 0 ?
                        0 :
                        Quick.AirSpeed;
                }
                else if (result == Direction.Right)
                {
                    float min = Math.Min(edge.begin.X, edge.end.X);
                    aabb.position.X = min - aabb.Width;
                    aabb.position.Y = target.Y;
                    velocity.X = this.velocity.X + angularVelocity.YAxis.X * width * Math.Abs(angularVelocity.Angle);
                }
                else if (result == Direction.Left)
                {
                    float max = Math.Max(edge.begin.X, edge.end.X);
                    aabb.position.X = max;
                    aabb.position.Y = target.Y;
                    velocity.X = this.velocity.X + angularVelocity.YAxis.X * width * Math.Abs(angularVelocity.Angle);
                }
                else if (Math.Tan(Math.Abs(rotation.Angle + MathHelper.PiOver2)) <= miu)
                {
                    Direction h = result & (Direction.Right | Direction.Left);
                    float w = Vector2.Dot(result.ToVector2() * aabb.size / 2 + aabb.Center - position, rotation.YAxis);
                    result = result & (~Direction.Right) & (~Direction.Left);
                    if ((rotation.Angle < 0 && result == Direction.Top) || (rotation.Angle > 0 && result == Direction.Bottom))
                    {
                        result = Direction.None;
                    }
                    else
                    {
                        aabb.position.X = target.X;
                        aabb.position.Y = (h == Direction.Left ? aabb.Left : aabb.Right) * edge.K + edge.B - (rotation.Angle > 0 ? 0 : aabb.Height);
                        cache = angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle) * Vector2.UnitY;
                        velocity.Y = this.velocity.Y + cache.Y;
                    }
                }
                else
                {
                    float w = Vector2.Dot((result.ToVector2() * aabb.size / 2 + aabb.Center) - position, rotation.YAxis);
                    velocity = Vector2.Dot(velocity, rotation.YAxis) * rotation.YAxis + this.velocity + angularVelocity.YAxis * w * Math.Abs(angularVelocity.Angle);
                }
                move = aabb.position - pos0 + this.velocity;
                break;
            }
        } while (aabb.position != target);
        return result;
    }
    public override void Draw()
    {
        Main.spriteBatch.Draw(TextureType.GetValue(),
            position - Main.screenPosition,
            new Rectangle(0, 0, 1, (int)width),
            Color.White,
            rotation.Angle,
            new Vector2(0, width / 2f), 1,
            SpriteEffects.None, 0);
    }
    public override void Move()
    {
        base.Move();
        rotation += angularVelocity;
    }
    public void SetHookPosition(Projectile hook)
    {
        float proj = Vector2.Dot(hook.Center - position, rotation.YAxis);
        hook.Center = position + rotation.YAxis * proj;
    }
    public override void Leave(EntityHandler handler)
    {
        //entity.velocity += velocity;
    }
    public override void Stand(EntityHandler handler, bool newStand)
    {
        handler.extraVelocity = cache + velocity;
    }
}