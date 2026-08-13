import React, { useEffect, useRef, useState } from "react";
import "./badgelist.css";

interface BadgeListProps {
    badges: string[];
}

function BadgeList({ badges }: BadgeListProps) {
    const containerRef = useRef<HTMLDivElement>(null);
    const [visibleCount, setVisibleCount] = useState(badges.length);
    const [expanded, setExpanded] = useState(false);

    useEffect(() => {
        if (!containerRef.current) return;

        const containerWidth = containerRef.current.offsetWidth;
        let usedWidth = 0;
        let count = 0;

        // Hidden measurement container
        const measurement = document.createElement("div");
        measurement.style.visibility = "hidden";
        measurement.style.position = "absolute";
        measurement.style.left = "-9999px";
        measurement.style.top = "-9999px";
        measurement.style.display = "flex";
        measurement.style.flexWrap = "nowrap";
        document.body.appendChild(measurement);

        for (let badge of badges) {
            const badgeEl = document.createElement("span");
            badgeEl.className = "badge-item";
            badgeEl.innerText = badge;
            measurement.appendChild(badgeEl);

            const width = badgeEl.offsetWidth + 8;

            if (usedWidth + width > containerWidth) break;

            usedWidth += width;
            count++;
        }

        document.body.removeChild(measurement);

        // ⭐ FIXED LOGIC ⭐

        // If only one badge exists, always show it
        if (badges.length === 1) {
            setVisibleCount(1);
            return;
        }

        // If none fit, show at least the first one
        if (count === 0) {
            setVisibleCount(1);
            return;
        }

        setVisibleCount(count);
    }, [badges]);

    const overflow = badges.length - visibleCount;

    return (
        <div className="badge-list-container" ref={containerRef}>
            {(expanded ? badges : badges.slice(0, visibleCount)).map((b, i) => (
                <span key={i} className="badge-item">
                    {b}
                </span>
            ))}

            {/* Only show +X more if at least one badge is visible */}
            {overflow > 0 && !expanded && visibleCount > 0 && (
                <span
                    className="badge-item badge-more"
                    onClick={() => setExpanded(true)}
                >
                    +{overflow} more
                </span>
            )}

            {expanded && overflow > 0 && (
                <span
                    className="badge-item badge-less"
                    onClick={() => setExpanded(false)}
                >
                    Show less
                </span>
            )}
        </div>
    );
}

export default BadgeList;
