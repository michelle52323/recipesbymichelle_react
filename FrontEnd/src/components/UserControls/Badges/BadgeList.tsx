import React, { useEffect, useRef, useState } from "react";
import "./badgelist.css";

interface BadgeListProps {
    badges: string[];
    badgeRowHeight: number;
    setBadgeRowHeight: (height: number) => void
}

function BadgeList({ badges, badgeRowHeight, setBadgeRowHeight }: BadgeListProps) {
    const containerRef = useRef<HTMLDivElement>(null);
    const [visibleCount, setVisibleCount] = useState(badges.length);
    const [expanded, setExpanded] = useState(false);

    useEffect(() => {
        if (!containerRef.current) return;

        // Calculate usable width
        const usableWidth = window.innerWidth * 0.92 - 170;

        // Create hidden measurement container
        const measurement = document.createElement("div");
        measurement.style.visibility = "hidden";
        measurement.style.position = "absolute";
        measurement.style.left = "-9999px";
        measurement.style.top = "-9999px";
        document.body.appendChild(measurement);

        // Measure total width of all badges
        let totalWidth = 0;

        for (let badge of badges) {
            const badgeEl = document.createElement("span");
            badgeEl.className = "badge-item";
            badgeEl.innerText = badge;
            measurement.appendChild(badgeEl);

            const textWidth = badgeEl.offsetWidth;
            const badgeWidth = textWidth + 22; // 16px padding + 6px gap
            totalWidth += badgeWidth;
        }

        document.body.removeChild(measurement);

        // Apply rule:
        // If all badges fit → show all
        // Else → show only 1 badge
        if (totalWidth <= usableWidth) {
            setVisibleCount(badges.length);
        } else {
            setVisibleCount(1);
        }

        // Calculate number of rows needed when expanded
        if (expanded) {
            // Include "Show less" badge in total width
            const measurement2 = document.createElement("span");
            measurement2.className = "badge-item";
            measurement2.innerText = "Show less";
            document.body.appendChild(measurement2);

            const showLessWidth = measurement2.offsetWidth + 22;
            document.body.removeChild(measurement2);

            const expandedTotalWidth = totalWidth + showLessWidth;

            const rowsNeeded = Math.ceil(expandedTotalWidth / usableWidth);

            setBadgeRowHeight(15 + rowsNeeded * 35);
        } else {
            // Collapsed mode is always one row
            setBadgeRowHeight(50);
        }

    }, [badges, expanded]);

    const overflow = badges.length - visibleCount;



    return (
        <>
            {badges.length > 0 && (
                <div className="badge-list-print-container">
                    <span className="category-badge-label">
                        Categories:&nbsp;
                    </span>

                    <span className="category-badge-pipe">
                        {badges.join(" | ")}
                    </span>
                </div>
            )}


            <div className="badge-list-container" ref={containerRef}>
                {(expanded ? badges : badges.slice(0, visibleCount)).map((b, i) => (
                    <span key={i} className="badge-item">
                        {b}
                    </span>
                ))}

                {!expanded && overflow > 0 && (
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
        </>

    );
}

export default BadgeList;
